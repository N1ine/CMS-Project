using System.Threading;
using System.Threading.Tasks;
using Shared.Requests;
using Shared.Responses;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly ICompanyService _companyService;

    public GetCompanyByIdQueryHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        => _companyService.GetByIdAsync(request.Id);
}
