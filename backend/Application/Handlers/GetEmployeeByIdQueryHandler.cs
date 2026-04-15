using System.Threading;
using System.Threading.Tasks;
using Shared.Requests;
using Shared.Responses;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private readonly IEmployeeService _employeeService;

    public GetEmployeeByIdQueryHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        => _employeeService.GetByIdAsync(request.Id);
}
