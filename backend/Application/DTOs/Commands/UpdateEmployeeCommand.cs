using MediatR;

namespace Application.DTOs.Commands;

public class UpdateEmployeeCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
}