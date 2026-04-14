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
public class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CompanyDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetCompanyByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CompanyDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCompaniesQuery());
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<ActionResult<int>> Create([FromBody] CreateCompanyCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyCommand command)
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
        await _mediator.Send(new DeleteCompanyCommand { Id = id });
        return NoContent();
    }
}