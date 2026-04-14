using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, Unit>
{
    private readonly IContractService _contractService;

    public DeleteContractCommandHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public async Task<Unit> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
    {
        await _contractService.DeleteAsync(request);
        return Unit.Value;
    }
}