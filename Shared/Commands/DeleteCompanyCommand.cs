using Shared.DTOs;
using Shared.Enums;
﻿using MediatR;

namespace Shared.Commands;

public class DeleteCompanyCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
