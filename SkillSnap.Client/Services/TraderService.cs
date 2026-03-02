using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SkillSnap.Client.Models;

public class TraderService
{
    private readonly HttpClient _httpClient;

    public TraderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Trader>> GetTradersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<TraderResponse>(
            "api/traders", 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return response?.Data?.Traders ?? new List<Trader>();
    }
}