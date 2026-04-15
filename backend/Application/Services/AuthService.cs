using Shared.DTOs;
using Shared.Commands;
using Shared.Responses;
using Application.Common.Security;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Threading.Tasks;

namespace Application.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmployeeRepository _employeeRepository;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmployeeRepository employeeRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _employeeRepository = employeeRepository;
    }

    public async Task<UserDto> RegisterAsync(RegisterUserCommand command)
    {
        if (command is null) throw new ValidationException("Command must not be null");
        if (string.IsNullOrWhiteSpace(command.UserName)) throw new ValidationException("UserName must not be empty");
        if (string.IsNullOrWhiteSpace(command.Password)) throw new ValidationException("Password must not be empty");

        var existing = await _userRepository.GetByUserNameAsync(command.UserName.Trim());
        if (existing != null) throw new ValidationException($"User with username '{command.UserName}' already exists");

        var userRoleId = await _userRepository.GetRoleIdByNameAsync("User");

        if (userRoleId == null)
            throw new ValidationException("Role 'User' not found in database. Please run seed script.");

        var passwordHash = _passwordHasher.HashPassword(command.Password);

        var user = new User
        {
            UserName = command.UserName.Trim(),
            PasswordHash = passwordHash,
            Role = "User",
            RoleId = userRoleId.Value 
        };

        var id = await _userRepository.CreateAsync(user);
        user.Id = id;

        if (command.EmployeeId.HasValue)
        {
            var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId.Value);
            if (employee == null)
                throw new ValidationException($"Employee with id {command.EmployeeId.Value} not found");

            await _userRepository.AddUserEmployeeAsync(id, command.EmployeeId.Value);
        }


        var employees = await _userRepository.GetEmployeeIdsByUserIdAsync(id);
        var linkedEmployeeId = employees?.FirstOrDefault();

        return new UserDto(user.Id, user.UserName, user.Role, linkedEmployeeId);
    }

    public async Task<AuthResultDto> LoginAsync(LoginUserCommand command)
    {
        if (command is null)
            throw new ValidationException("Command must not be null");

        if (string.IsNullOrWhiteSpace(command.UserName) ||
            string.IsNullOrWhiteSpace(command.Password))
        {
            throw new ValidationException("UserName and Password must not be empty");
        }

        var user = await _userRepository.GetByUserNameAsync(command.UserName.Trim());
        if (user == null)
            throw new ValidationException("Invalid username or password");

        var validPassword = _passwordHasher.VerifyPassword(user.PasswordHash, command.Password);
        if (!validPassword)
            throw new ValidationException("Invalid username or password");

        var authResult = _jwtTokenGenerator.GenerateToken(user);
        return authResult;
    }
}
