using Application.DTOs.Commands;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.Handlers;
public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, int>
{
    private readonly IContractService _contractService;

    public CreateContractCommandHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public Task<int> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        => _contractService.CreateAsync(request);
}