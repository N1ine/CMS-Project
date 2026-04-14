using System.Net.Http.Json;
using Client.Models;
using Client.Models.Commands;

namespace Client.Services;

public class EmployeeService
{
    private readonly HttpClient _httpClient;

    public EmployeeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        // Твой контроллер не имеет простого GET. 
        // Поэтому мы используем твой метод search без параметров, чтобы получить всех.
        return await _httpClient.GetFromJsonAsync<List<EmployeeDto>>("api/employees/search?firstName=&lastName=") ?? new();
    }

    public async Task<List<EmployeeDto>> GetByCompanyIdAsync(int companyId)
    {
        // Меняем URL в точности так, как указано в твоем контроллере: [HttpGet("by-company/{companyId:int}")]
        return await _httpClient.GetFromJsonAsync<List<EmployeeDto>>($"api/employees/by-company/{companyId}") ?? new();
    }

    public async Task<int> CreateAsync(CreateEmployeeCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/employees", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task UpdateAsync(int id, UpdateEmployeeCommand command)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/employees/{id}", command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/employees/{id}");
        response.EnsureSuccessStatusCode();
    }
}
