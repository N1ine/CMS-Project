using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

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