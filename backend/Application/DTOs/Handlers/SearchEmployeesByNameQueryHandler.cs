using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Requests;
// Интерфейс IEmployeeService у тебя, судя по всему, лежит без неймспейса Application.Interfaces (или в корне), 
// но если студия подчеркнет его красным, добавь: using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

public class SearchEmployeesByNameQueryHandler : IRequestHandler<SearchEmployeesByNameQuery, IReadOnlyList<EmployeeDto>>
{
    private readonly IEmployeeService _employeeService;

    public SearchEmployeesByNameQueryHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public Task<IReadOnlyList<EmployeeDto>> Handle(SearchEmployeesByNameQuery request, CancellationToken cancellationToken)
    {
        // Передаем целиком объект request, как того требует твой IEmployeeService!
        return _employeeService.SearchByNameAsync(request);
    }
}
