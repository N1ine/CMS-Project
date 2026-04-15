using MediatR;
using Shared.DTOs;

namespace Shared.Requests;

public class GetCompanyByIdQuery : IRequest<CompanyDto>
{
    public int Id { get; set; }
}

public class GetAllCompaniesQuery : IRequest<IReadOnlyList<CompanyDto>>
{
}
