using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.DTOs.Handlers;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly IEmployeeService _employeeService;

    public DeleteEmployeeCommandHandler(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await _employeeService.DeleteAsync(request.Id);
        return Unit.Value;
    }
}