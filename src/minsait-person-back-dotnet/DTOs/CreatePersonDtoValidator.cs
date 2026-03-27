using FluentValidation;
using System.Text.RegularExpressions;

namespace MinsaitPersonBack.DTOs;

public class CreatePersonDtoValidator : AbstractValidator<CreatePersonDto>
{
    private static readonly string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public CreatePersonDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 120);
        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(EmailPattern)
            .WithMessage("'{PropertyName}' must be a valid email address with a domain name.")
            .MaximumLength(180);
    }
}
