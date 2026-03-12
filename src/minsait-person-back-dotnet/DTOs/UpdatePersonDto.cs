
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace MinsaitPersonBack.DTOs;

public class UpdatePersonDto
{
    /// <summary>
    /// Full name of the person
    /// </summary>
    /// <example>Ada Lovelace</example>
    [StringLength(120)]
    public string? Name { get; set; }

    /// <summary>
    /// Email address of the person
    /// </summary>
    /// <example>ada@example.com</example>
    [EmailAddress]
    [StringLength(180)]
    public string? Email { get; set; }



    public void ValidateAndThrow()
    {
        var validator = new UpdatePersonDtoValidator();
        var result = validator.Validate(this);
        if (!result.IsValid)
            throw new FluentValidation.ValidationException(result.Errors);
    }
}
