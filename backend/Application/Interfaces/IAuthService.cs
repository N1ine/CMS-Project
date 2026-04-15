using Shared.DTOs;
using Shared.Commands;
using Shared.Responses;
using System.Threading.Tasks;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterUserCommand command);
    Task<AuthResultDto> LoginAsync(LoginUserCommand command);
}
