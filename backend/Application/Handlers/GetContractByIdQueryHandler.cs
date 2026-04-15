using Shared.DTOs;
using Shared.Requests;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.DTOs.Handlers;

public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, ContractDto>
{
    private readonly IContractService _contractService;

    public GetContractByIdQueryHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public Task<ContractDto> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        => _contractService.GetByIdAsync(request.Id);
}
