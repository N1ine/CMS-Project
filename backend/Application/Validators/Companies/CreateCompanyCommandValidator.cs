using Application.DTOs.Commands;
using FluentValidation;

namespace Application.Validation.Companies;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.TaxNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.TaxNumber));
    }
}