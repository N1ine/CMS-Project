using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

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