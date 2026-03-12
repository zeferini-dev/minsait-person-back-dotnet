using System.ComponentModel.DataAnnotations;


using FluentValidation;
namespace MinsaitPersonBack.Models;

public class Person
{
    /// <summary>
    /// Unique identifier for the person
    /// </summary>
    /// <example>123e4567-e89b-12d3-a456-426614174000</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Full name of the person
    /// </summary>
    /// <example>Ada Lovelace</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the person
    /// </summary>
    /// <example>ada@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the person was created
    /// </summary>
    /// <example>2025-01-01T12:00:00.000Z</example>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the person was last updated
    /// </summary>
    /// <example>2025-01-02T12:00:00.000Z</example>
    public DateTime UpdatedAt { get; set; }



    public void ValidateAndThrow()
    {
        var validator = new PersonValidator();
        var result = validator.Validate(this);
        if (!result.IsValid)
            throw new FluentValidation.ValidationException(result.Errors);
    }
}
