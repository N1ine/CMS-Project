using Application.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.DTOs.Requests;

public class GetContractByIdQuery : IRequest<ContractDto>
{
    public int Id { get; set; }
}

public class GetAllContractsQuery : IRequest<IReadOnlyList<ContractDto>>
{
    public int? EmployeeId { get; set; }
}

public class SearchContractsByPositionQuery : IRequest<IReadOnlyList<ContractDto>>
{
    public string Position { get; set; } = null!;
}

public class SearchContractsByEmployeeNameQuery : IRequest<IReadOnlyList<ContractDto>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class GetContractsByStatusQuery : IRequest<IReadOnlyList<ContractDto>>
{
    public ContractStatus Status { get; set; }
}

public class GetContractsByDateRangeQuery : IRequest<IReadOnlyList<ContractDto>>
{
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public DateTime? EndDateFrom { get; set; }
    public DateTime? EndDateTo { get; set; }
}