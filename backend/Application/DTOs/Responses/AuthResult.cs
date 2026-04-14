namespace Application.DTOs.Responses;

public record AuthResultDto(
    string AccessToken,
    DateTime ExpiresAt,
    UserDto User
);
