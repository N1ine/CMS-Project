using System.Threading;
using System.Threading.Tasks;
using Shared.Commands;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, Unit>
{
    private readonly IContractService _contractService;

    public UpdateContractCommandHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
        await _contractService.UpdateAsync(request);
        return Unit.Value;
    }
}
