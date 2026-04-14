namespace Client.Models.Commands;

public class CreateContractCommand
{
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }
    public string Position { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; }
    public decimal Wage { get; set; }
}
