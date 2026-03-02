using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TradersController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    public TradersController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetTraders()
    {
        var httpClient = _httpClientFactory.CreateClient(); 

        try
        {
            var queryObject = new
            {
                query = @"query {
                            traders {
                                id
                                name
                                description
                                imageLink
                            }
                        }"
            };

            var response = await httpClient.PostAsJsonAsync("https://api.tarkov.dev/graphql", queryObject);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Tarkov API error: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Server error: {ex.Message}");
        }
    }
}