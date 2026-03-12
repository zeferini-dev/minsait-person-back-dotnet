using Xunit;
using MinsaitPersonBack.DTOs;
using FluentValidation;
using FluentAssertions;

public class CreatePersonDtoTests
{
    [Fact]
    public void CreatePersonDto_ShouldInitialize_WithEmptyValues()
    {
        // Arrange & Act
        var dto = new CreatePersonDto();

        // Assert
        dto.Name.Should().BeEmpty();
        dto.Email.Should().BeEmpty();
    }

    [Fact]
    public void CreatePersonDto_ShouldSetProperties_Correctly()
    {
        // Arrange & Act
        var dto = new CreatePersonDto
        {
            Name = "Ada Lovelace",
            Email = "ada@example.com"
        };

        // Assert
        dto.Name.Should().Be("Ada Lovelace");
        dto.Email.Should().Be("ada@example.com");
    }

    [Fact]
    public void ValidateAndThrow_ValidDto_DoesNotThrow()
    {
        var dto = new CreatePersonDto { Name = "Ada Lovelace", Email = "ada@example.com" };
        var act = () => dto.ValidateAndThrow();
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateAndThrow_InvalidName_ThrowsValidationException()
    {
        var dto = new CreatePersonDto { Name = "", Email = "ada@example.com" };
        var act = () => dto.ValidateAndThrow();
        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Name"));
    }

    [Fact]
    public void ValidateAndThrow_InvalidEmail_ThrowsValidationException()
    {
        var dto = new CreatePersonDto { Name = "Ada Lovelace", Email = "not-an-email" };
        var act = () => dto.ValidateAndThrow();
        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Email"));
    }

    [Fact]
    public void ValidateAndThrow_NameTooLong_ThrowsValidationException()
    {
        var dto = new CreatePersonDto 
        { 
            Name = new string('a', 121), 
            Email = "ada@example.com" 
        };
        var act = () => dto.ValidateAndThrow();
        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Name"));
    }

    [Fact]
    public void ValidateAndThrow_EmailTooLong_ThrowsValidationException()
    {
        var dto = new CreatePersonDto 
        { 
            Name = "Ada Lovelace", 
            Email = new string('a', 181) + "@example.com" 
        };
        var act = () => dto.ValidateAndThrow();
        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Email"));
    }

    [Fact]
    public void ValidateAndThrow_NameAtMaxLength_DoesNotThrow()
    {
        var dto = new CreatePersonDto
        {
            Name = new string('a', 120),
            Email = "ada@example.com"
        };
        var act = () => dto.ValidateAndThrow();
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateAndThrow_NameAtMinLength_DoesNotThrow()
    {
        var dto = new CreatePersonDto
        {
            Name = "a",
            Email = "ada@example.com"
        };
        var act = () => dto.ValidateAndThrow();
        act.Should().NotThrow();
    }
}