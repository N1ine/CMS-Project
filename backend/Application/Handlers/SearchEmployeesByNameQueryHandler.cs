using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.DTOs;
using Shared.Requests;
using MediatR;

namespace Shared.DTOs.Handlers;

public class SearchEmployeesByNameQueryHandler : IRequestHandler<SearchEmployeesByNameQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeService _employeeService;

    public SearchEmployeesByNameQueryHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public Task<IReadOnlyList<EmployeeDto>> Handle(SearchEmployeesByNameQuery request, CancellationToken cancellationToken)
    {
        return _employeeService.SearchByNameAsync(request);
    }
}
