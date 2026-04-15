using System.Net.Http.Json;
using global::Shared.DTOs;
using global::Shared.Commands;

namespace Client.Services;

public class CompanyService
{
    private readonly HttpClient _httpClient;

    public CompanyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CompanyDto>> GetAllCompaniesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CompanyDto>>("api/companies") 
               ?? new List<CompanyDto>();
    }

    public async Task<int> CreateCompanyAsync(CreateCompanyCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/companies", command);
        response.EnsureSuccessStatusCode();
        
        // Предполагаем, что бекенд возвращает ID созданной сущности
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/companies/{id}");
        response.EnsureSuccessStatusCode();
    }
}
