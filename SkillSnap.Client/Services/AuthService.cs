using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using SkillSnap.Shared.Models;

namespace SkillSnap.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    private const string TOKEN_KEY = "authToken";

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> Login(string email, string password)
    {
        var payload = new LoginRequest { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", payload); 
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResult>();
        if(result == null)
        {
            return false;
        }


        await _localStorage.SetItemAsync(TOKEN_KEY, result.Token);

        ((ApiAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

        return true;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync(TOKEN_KEY);
        ((ApiAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> Register(string userName, string email, string password)
    {
        var payload = new RegisterRequest { Username = userName, Email = email, Password = password };

        var response = await _httpClient.PostAsJsonAsync("api/auth/register", payload);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync<string>(TOKEN_KEY);
    }
}

public class LoginResult
{
    public string Token { get; set; } = string.Empty;
}