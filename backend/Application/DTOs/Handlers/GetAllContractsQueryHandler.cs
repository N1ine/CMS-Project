using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Handlers;

public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, IReadOnlyList<ContractDto>>
{
    private readonly IContractService _contractService;

    public GetAllContractsQueryHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public async Task<IReadOnlyList<ContractDto>> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
    {
        return await _contractService.GetAllAsync();
    }
}