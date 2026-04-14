using MudBlazor;

namespace Client.Services;

public class SearchFilters
{
    public string? EmployeeName { get; set; }
    public string? Position { get; set; }
    public int? Status { get; set; }
    public DateRange? StartDateRange { get; set; }
    public DateRange? EndDateRange { get; set; }
}

public class SearchService
{
    public SearchFilters Filters { get; private set; } = new();
    public event Action? OnSearchTriggered;

    public void TriggerSearch(SearchFilters newFilters)
    {
        Filters = newFilters;
        OnSearchTriggered?.Invoke();
    }
}