using Shared.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AdminController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.RoleName))
            return BadRequest("Invalid request");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return NotFound();

        var allowedRoles = new[] { "User", "Admin" };
        if (!allowedRoles.Contains(request.RoleName, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Role not allowed");

        var roleId = await _userRepository.GetRoleIdByNameAsync(request.RoleName);
        if (!roleId.HasValue)
            return BadRequest("Role not found in database");

        user.RoleId = roleId.Value;
        user.Role = request.RoleName;

        await _userRepository.UpdateAsync(user);

        return NoContent();
    }
}
