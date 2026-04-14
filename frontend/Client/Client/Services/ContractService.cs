using System.Net.Http.Json;
using Client.Models.DTOs;
using Client.Models.Commands;

namespace Client.Services;

public class ContractsService
{
    private readonly HttpClient _httpClient;

    public ContractsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ContractDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ContractDto>>("api/contracts") 
               ?? new List<ContractDto>();
    }

    // ВОТ ЭТОТ МЕТОД НУЖЕН БЫЛ КОМПИЛЯТОРУ
    public async Task<int> CreateAsync(CreateContractCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/contracts", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task UpdateAsync(int id, UpdateContractCommand command)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/contracts/{id}", command);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/contracts/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ContractDto>> GetFilteredAsync(SearchFilters filters)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(filters.EmployeeName))
            queryParams.Add($"employeeName={Uri.EscapeDataString(filters.EmployeeName)}");

        if (!string.IsNullOrWhiteSpace(filters.Position))
            queryParams.Add($"position={Uri.EscapeDataString(filters.Position)}");

        if (filters.Status.HasValue)
            queryParams.Add($"status={filters.Status.Value}");

        // Обработка дат начала
        if (filters.StartDateRange?.Start != null)
            queryParams.Add($"startDateFrom={filters.StartDateRange.Start.Value:yyyy-MM-dd}");
        if (filters.StartDateRange?.End != null)
            queryParams.Add($"startDateTo={filters.StartDateRange.End.Value:yyyy-MM-dd}");

        // Обработка дат окончания
        if (filters.EndDateRange?.Start != null)
            queryParams.Add($"endDateFrom={filters.EndDateRange.Start.Value:yyyy-MM-dd}");
        if (filters.EndDateRange?.End != null)
            queryParams.Add($"endDateTo={filters.EndDateRange.End.Value:yyyy-MM-dd}");

        var queryString = string.Join("&", queryParams);
        var url = string.IsNullOrEmpty(queryString) ? "api/contracts/search" : $"api/contracts/search?{queryString}";

        return await _httpClient.GetFromJsonAsync<List<ContractDto>>(url) ?? new List<ContractDto>();
    }
}
