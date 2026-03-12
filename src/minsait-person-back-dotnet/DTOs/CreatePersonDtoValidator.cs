using FluentValidation;

namespace MinsaitPersonBack.DTOs;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    public CreatePersonDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 120);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(180);
    }
}
