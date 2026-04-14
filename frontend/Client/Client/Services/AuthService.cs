using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

// Явно подключаем пространства имен твоих моделей
using Client.Models;
using Client.Models.Commands;

namespace Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    // Явно требуем LoginCommand из папки Models.Commands
    public async Task<bool> LoginAsync(Client.Models.Commands.LoginCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", command); 

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResultDto>();
            if (result != null && !string.IsNullOrEmpty(result.AccessToken))
            {
                await _localStorage.SetItemAsync("authToken", result.AccessToken);
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
                return true;
            }
        }
        return false;
    }

    // Явно требуем RegisterUserCommand из папки Models.Commands
    public async Task<bool> RegisterAsync(Client.Models.Commands.RegisterUserCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", command);
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
