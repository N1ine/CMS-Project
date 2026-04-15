using Shared.DTOs;
using Shared.Commands;
using Shared.Requests;
using Shared.Responses;
using Shared.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContractDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetContractByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractDto>>> GetAll()
    {
        var query = new GetAllContractsQuery();

        if (User.IsInRole("User"))
        {
            var employeeIdClaim = User.FindFirst("employeeId");

            if (employeeIdClaim != null && int.TryParse(employeeIdClaim.Value, out int empId))
            {
                query.EmployeeId = empId;
            }
            else
            {
                return Forbid();
            }
        }

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("search/by-position")]
    public async Task<ActionResult<IReadOnlyList<ContractDto>>> SearchByPosition([FromQuery] string position)
    {
        var result = await _mediator.Send(new SearchContractsByPositionQuery { Position = position });
        return Ok(result);
    }

    [HttpGet("search/by-employee")]
    public async Task<ActionResult<IReadOnlyList<ContractDto>>> SearchByEmployee(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName)
    {
        var result = await _mediator.Send(new SearchContractsByEmployeeNameQuery
        {
            FirstName = firstName,
            LastName = lastName
        });
        return Ok(result);
    }

    [HttpGet("by-status")]
    public async Task<ActionResult<IReadOnlyList<ContractDto>>> GetByStatus([FromQuery] string status)
    {
        if (!Enum.TryParse(status, true, out Shared.Enums.ContractStatus parsed))
            return BadRequest("Invalid status");

        var result = await _mediator.Send(new GetContractsByStatusQuery { Status = parsed });
        return Ok(result);
    }

    [HttpGet("by-dates")]
    public async Task<ActionResult<IReadOnlyList<ContractDto>>> GetByDates(
        [FromQuery] DateTime? startDateFrom,
        [FromQuery] DateTime? startDateTo,
        [FromQuery] DateTime? endDateFrom,
        [FromQuery] DateTime? endDateTo)
    {
        var query = new GetContractsByDateRangeQuery
        {
            StartDateFrom = startDateFrom,
            StartDateTo = startDateTo,
            EndDateFrom = endDateFrom,
            EndDateTo = endDateTo
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateContractCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContractCommand command)
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
        await _mediator.Send(new DeleteContractCommand { Id = id });
        return NoContent();
    }
}
