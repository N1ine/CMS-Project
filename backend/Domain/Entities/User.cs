namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RoleId {  get; set; }
    public string Role { get; set; } = "Employee";
    public int? EmployeeId { get; set; }
}