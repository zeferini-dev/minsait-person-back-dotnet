using FluentValidation;

namespace MinsaitPersonBack.Models;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
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
