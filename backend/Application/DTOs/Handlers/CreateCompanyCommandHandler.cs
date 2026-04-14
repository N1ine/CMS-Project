using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, int>
{
    private readonly ICompanyService _companyService;

    public CreateCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        => _companyService.CreateAsync(request);
}