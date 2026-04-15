using MediatR;
using Shared.DTOs;

namespace Shared.Requests;

public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
{
    public int Id { get; set; }
}

public class GetEmployeesByCompanyQuery : IRequest<IReadOnlyList<EmployeeDto>>
{
    public int CompanyId { get; set; }
}

// ИСПРАВЛЕНО: Теперь запрос возвращает список сотрудников, а не одного
public class SearchEmployeesByNameQuery : IRequest<IReadOnlyList<EmployeeDto>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
