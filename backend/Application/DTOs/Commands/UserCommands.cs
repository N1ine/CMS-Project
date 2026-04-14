namespace Application.DTOs.Commands;

public class RegisterUserCommand
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User";
    public int? EmployeeId { get; set; }
}

public class LoginCommand
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}