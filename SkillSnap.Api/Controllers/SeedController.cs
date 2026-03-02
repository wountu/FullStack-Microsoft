using Microsoft.AspNetCore.Mvc;
using SkillSnap.Shared.Models;
namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly SkillSnapContext _context;
        public SeedController(SkillSnapContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Seed()
        {
            if (_context.Users.Any())
            {
                return BadRequest("Sample data already exists.");
            }
            var user = new ApplicationUser
            {
                UserName = "Jordan",
                Email = "gmail",
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Sample data inserted.");
        }
    }
}