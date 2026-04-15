using System;
using System.Text.Json.Serialization; 
using Shared.Enums;

namespace Shared.DTOs;

public class ContractDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }

    public string CompanyName { get; set; } = string.Empty;
    public string EmployeeFirstName { get; set; } = string.Empty;
    public string EmployeeLastName { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public decimal Wage { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContractStatus Status { get; set; } 
}
