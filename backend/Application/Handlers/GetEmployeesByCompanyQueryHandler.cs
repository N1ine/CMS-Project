using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Requests;
using Shared.Responses;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

public class GetEmployeesByCompanyQueryHandler : IRequestHandler<GetEmployeesByCompanyQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeService _employeeService;

    public GetEmployeesByCompanyQueryHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public Task<IReadOnlyList<EmployeeDto>> Handle(GetEmployeesByCompanyQuery request, CancellationToken cancellationToken)
        => _employeeService.GetByCompanyAsync(request);
}
