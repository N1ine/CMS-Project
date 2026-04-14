using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Commands;
using MediatR;

namespace Application.DTOs.Handlers;

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