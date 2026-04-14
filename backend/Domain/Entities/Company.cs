namespace Domain.Entities;
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
}