namespace Client.Models;

public record UserDto(int Id, string UserName, string Role, int? EmployeeId);
public record AuthResultDto(string AccessToken, DateTime ExpiresAt, UserDto User);
