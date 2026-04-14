using Application.DTOs;
using Application.DTOs.Commands;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetEmployeeByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("by-company/{companyId:int}")]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetByCompany(int companyId)
    {
        var result = await _mediator.Send(new GetEmployeesByCompanyQuery { CompanyId = companyId });
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> Search(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName)
    {
        var result = await _mediator.Send(new SearchEmployeesByNameQuery
        {
            FirstName = firstName,
            LastName = lastName
        });
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateEmployeeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route id and body id must match");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteEmployeeCommand { Id = id });
        return NoContent();
    }
}
