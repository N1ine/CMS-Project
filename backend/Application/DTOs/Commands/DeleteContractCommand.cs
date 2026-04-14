using MediatR;

namespace Application.DTOs.Commands;

public class DeleteContractCommand : IRequest<Unit>
{
    public int Id { get; set; }
}