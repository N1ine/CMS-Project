using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Requests;
using Shared.Responses;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

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
