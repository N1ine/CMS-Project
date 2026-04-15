using Shared.Commands;
using FluentValidation;

namespace Application.Validation.Contracts;

public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
{
    public CreateContractCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0);

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0);

        RuleFor(x => x.Position)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Wage)
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x)
            .Must(x => !x.EndDate.HasValue || x.EndDate.Value >= x.StartDate)
            .WithMessage("EndDate must be greater than or equal to StartDate");
    }
}
