using Shared.DTOs;
using Shared.Requests;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.DTOs.Handlers;

public class SearchContractsByPositionQueryHandler
    : IRequestHandler<SearchContractsByPositionQuery, IReadOnlyList<ContractDto>>
{
    private readonly IContractService _contractService;

    public SearchContractsByPositionQueryHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public Task<IReadOnlyList<ContractDto>> Handle(
        SearchContractsByPositionQuery request,
        CancellationToken cancellationToken)
        => _contractService.SearchByPositionAsync(request);
}

public class SearchContractsByEmployeeNameQueryHandler
    : IRequestHandler<SearchContractsByEmployeeNameQuery, IReadOnlyList<ContractDto>>
{
    private readonly IContractService _contractService;

    public SearchContractsByEmployeeNameQueryHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public Task<IReadOnlyList<ContractDto>> Handle(
        SearchContractsByEmployeeNameQuery request,
        CancellationToken cancellationToken)
        => _contractService.SearchByEmployeeNameAsync(request);
}

public class GetContractsByStatusQueryHandler
    : IRequestHandler<GetContractsByStatusQuery, IReadOnlyList<ContractDto>>
{
    private readonly IContractService _contractService;

    public GetContractsByStatusQueryHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public Task<IReadOnlyList<ContractDto>> Handle(
        GetContractsByStatusQuery request,
        CancellationToken cancellationToken)
        => _contractService.GetByStatusAsync(request);
}

public class GetContractsByDateRangeQueryHandler
    : IRequestHandler<GetContractsByDateRangeQuery, IReadOnlyList<ContractDto>>
{
    private readonly IContractService _contractService;

    public GetContractsByDateRangeQueryHandler(IContractService contractService)
    {
        _contractService = contractService;
    }

    public Task<IReadOnlyList<ContractDto>> Handle(
        GetContractsByDateRangeQuery request,
        CancellationToken cancellationToken)
        => _contractService.GetByDateRangeAsync(request);
}
