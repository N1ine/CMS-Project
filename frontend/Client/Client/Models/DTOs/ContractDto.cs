using System;
// 1. ОБЯЗАТЕЛЬНО ДОБАВЬ ЭТУ СТРОЧКУ НАВЕРХУ:
using System.Text.Json.Serialization; 

namespace Client.Models.DTOs; // Твой неймспейс

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

    // 2. ДОБАВЬ ЭТОТ АТРИБУТ ПРЯМО НАД СВОЙСТВОМ STATUS
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ContractStatus Status { get; set; } 
}
