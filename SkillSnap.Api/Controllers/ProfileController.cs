using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace SkillSnap.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly SkillSnapContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMemoryCache _cache;

    public ProfilesController(SkillSnapContext context, UserManager<ApplicationUser> userManager, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        if (!_cache.TryGetValue("Profiles", out List<Profile>? cachedProfiles))
        {
            cachedProfiles = await _context.Profiles.AsNoTracking().ToListAsync();

            _cache.Set("Profiles", cachedProfiles, TimeSpan.FromMinutes(5));
        }

        return Ok(cachedProfiles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var cacheKey = $"Profile_{id}";
        if (!_cache.TryGetValue(cacheKey, out Profile? profile))
        {
            profile = await _context.Profiles.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
        if (profile == null)
        {
            return NotFound();
        }

        _cache.Set(cacheKey, profile, TimeSpan.FromMinutes(5));

        return Ok(profile); 
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetProfilesByUser(string userId)
    {
        var cacheKey = $"Profiles_User_{userId}";
        if (!_cache.TryGetValue(cacheKey, out List<Profile>? cachedProfiles))
        {
            cachedProfiles = await _context.Profiles.AsNoTracking().Where(p => p.ApplicationUserId == userId).ToListAsync();

            _cache.Set(cacheKey, cachedProfiles, TimeSpan.FromMinutes(5));
        }

        return Ok(cachedProfiles);
    }

    [HttpPost]
    public async Task<ActionResult<Profile>> CreateProfile(Profile profile)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if(currentUser == null)
            return Unauthorized();

        profile.ApplicationUserId = currentUser.Id;

        var levelQuery = @"
        query level {
            playerLevels {
                levelBadgeImageLink
                level
            }
            prestige {
                imageLink
                prestigeLevel
            }
        }";

        var responseJson = await GetTarkovResponse(levelQuery);

        var graphResponse = JsonSerializer.Deserialize<GraphQLResponse>(
            responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

        var matchingLevel = graphResponse?.Data?.PlayerLevels?.FirstOrDefault(l => l.Level == profile.Level);

        if(matchingLevel != null)
        {
            profile.LevelBadge = matchingLevel.LevelBadgeImageLink;
        }

        var matchingPrestige = graphResponse?.Data?.Prestige?.FirstOrDefault(p => p.PrestigeLevel == profile.PrestigeLevel);

        if(matchingPrestige != null)
        {
            Console.WriteLine(matchingPrestige);
            profile.PrestigeBadge = matchingPrestige.ImageLink;
        }
        else Console.WriteLine("No Prestige found");

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();

        var cacheKey = $"Profiles_User_{profile.ApplicationUserId}";
        _cache.Remove(cacheKey);

        return CreatedAtAction(nameof(GetProfiles), new { id = profile.Id}, profile);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfile(int id)
    {
        var profile = await _context.Profiles.FindAsync(id);

        if(profile == null)
        return NotFound();

        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();

        var cacheKey = $"Profiles_User_{profile.ApplicationUserId}";
        _cache.Remove(cacheKey);

        return NoContent();
    }

    private async Task<string> GetTarkovResponse(string query)
    {
        var client = new HttpClient();

        var requestBody = new
        {
            query = query
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://api.tarkov.dev/graphql", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public class GraphQLResponse 
    {
        public GraphQLData Data {get; set; }
    }

    public class GraphQLData
    {
        public List<PlayerLevel> PlayerLevels { get; set; }
        public List<Prestige> Prestige { get; set; }
    }

    public class PlayerLevel
    {
        public int Level { get; set; }
        public string LevelBadgeImageLink { get; set; }
    }

    public class Prestige
    {
        public int PrestigeLevel  { get; set; }
        public string ImageLink  { get; set; }
    }
}