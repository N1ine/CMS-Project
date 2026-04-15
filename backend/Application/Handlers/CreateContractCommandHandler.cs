using Shared.Commands;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.DTOs.Handlers;
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
