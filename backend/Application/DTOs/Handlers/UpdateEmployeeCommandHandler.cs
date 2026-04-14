using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Commands;
using MediatR;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
{
    private readonly IEmployeeService _employeeService;

    public UpdateEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _employeeService.UpdateAsync(request);
        return Unit.Value;
    }
}