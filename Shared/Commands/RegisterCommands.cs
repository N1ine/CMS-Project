using Shared.DTOs;
using Shared.Enums;

namespace Shared.Commands;

public class RegisterUserCommand
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User";
    public int? EmployeeId { get; set; }
}
