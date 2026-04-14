using Application.DTOs;
using Application.DTOs.Commands;
using Application.DTOs.Responses;
using System.Threading.Tasks;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterUserCommand command);
    Task<AuthResultDto> LoginAsync(LoginCommand command);
}