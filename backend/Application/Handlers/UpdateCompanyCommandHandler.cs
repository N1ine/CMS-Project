using System.Threading;
using System.Threading.Tasks;
using Shared.Commands;
using Application.Interfaces;
using MediatR;

namespace Shared.DTOs.Handlers;

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Unit>
{
    private readonly ICompanyService _companyService;

    public UpdateCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        await _companyService.UpdateAsync(request);
        return Unit.Value;
    }
}
