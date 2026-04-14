using MediatR;

namespace Application.DTOs.Commands;

public class DeleteCompanyCommand : IRequest<Unit>
{
    public int Id { get; set; }
}