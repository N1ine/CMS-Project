using Shared.DTOs;
using Shared.Enums;

namespace Shared.Commands;

public class LoginUserCommand
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
