using System.Threading;
using System.Threading.Tasks;
using Shared.Commands;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Unit>
{
    private readonly ICompanyService _companyService;

    public DeleteCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        await _companyService.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
