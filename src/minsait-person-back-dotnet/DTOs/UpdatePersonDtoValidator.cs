using FluentValidation;
using System.Text.RegularExpressions;

namespace MinsaitPersonBack.DTOs;

public class UpdatePersonDtoValidator : AbstractValidator<UpdatePersonDto>
{
    private static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public UpdatePersonDtoValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(120);
        RuleFor(x => x.Email)
            .Matches(EmailPattern)
            .WithMessage("'{PropertyName}' must be a valid email address with a domain name.")
            .MaximumLength(180)
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
