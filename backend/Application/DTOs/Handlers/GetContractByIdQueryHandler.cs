using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Handlers;

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
