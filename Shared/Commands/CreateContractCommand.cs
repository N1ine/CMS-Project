using Shared.DTOs;
using Shared.Enums;
﻿using MediatR;

namespace Shared.Commands;

public class CreateContractCommand : IRequest<int>
{
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }

    public string Position { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public decimal Wage { get; set; }
}
