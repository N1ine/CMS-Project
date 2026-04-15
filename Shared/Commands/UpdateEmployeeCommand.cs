using Shared.DTOs;
using Shared.Enums;
﻿using MediatR;

namespace Shared.Commands;

public class UpdateEmployeeCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? Email { get; set; }
}
