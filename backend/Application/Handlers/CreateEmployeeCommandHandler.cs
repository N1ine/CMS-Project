using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Commands;
using MediatR;

namespace Shared.DTOs.Handlers;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
{
    private readonly IEmployeeService _employeeService;

    public CreateEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        => _employeeService.CreateAsync(request);
}
