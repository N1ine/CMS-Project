namespace Client.Models.Commands;

public class CreateEmployeeCommand
{
    public int CompanyId { get; set; } // Убрали ?
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
}
