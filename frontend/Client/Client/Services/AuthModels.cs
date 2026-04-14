namespace Client.Services;

public class LoginCommand
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class RegisterCommand
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public record UserDto(int Id, string UserName, string Role, int? EmployeeId);
public record AuthResultDto(string AccessToken, DateTime ExpiresAt, UserDto User);