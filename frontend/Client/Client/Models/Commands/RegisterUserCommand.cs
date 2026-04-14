namespace Client.Models.Commands;

public record RegisterUserCommand(string FirstName, string LastName, string? Email, string UserName, string Password, int? EmployeeId = null);
