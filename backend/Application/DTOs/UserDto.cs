namespace Application.DTOs;

public record UserDto(
    int Id,
    string UserName,
    string Role,
    int? EmployeeId
);