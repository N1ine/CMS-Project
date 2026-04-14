using MediatR;

namespace Application.DTOs.Commands;

public class DeleteEmployeeCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

