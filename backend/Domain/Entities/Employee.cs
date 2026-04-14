namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }

    public Company? Company { get; set; }
}