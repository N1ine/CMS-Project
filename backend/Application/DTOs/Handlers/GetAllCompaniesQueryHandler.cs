using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, IReadOnlyList<CompanyDto>>
{
	private readonly ICompanyService _companyService;

	public GetAllCompaniesQueryHandler(ICompanyService companyService)
	{
		_companyService = companyService;
	}

	public Task<IReadOnlyList<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
		=> _companyService.GetAllAsync();
}