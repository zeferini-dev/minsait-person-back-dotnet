using FluentValidation;

namespace MinsaitPersonBack.DTOs;

public class UpdatePersonDtoValidator : AbstractValidator<UpdatePersonDto>
{
    public UpdatePersonDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(120);
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(180)
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
