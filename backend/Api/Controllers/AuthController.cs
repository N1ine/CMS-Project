using Application.DTOs;
using Application.DTOs.Commands;
using Application.DTOs.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserCommand command)
    {
        var user = await _authService.RegisterAsync(command);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginCommand command)
    {
        var result = await _authService.LoginAsync(command);
        return Ok(result);
    }
}