using System.Net;
using MediatR;

namespace Application.DTOs.Requests;

public class GetCompanyByIdQuery : IRequest<CompanyDto>
{
    public int Id { get; set; }
}

public class GetAllCompaniesQuery : IRequest<IReadOnlyList<CompanyDto>>
{
}