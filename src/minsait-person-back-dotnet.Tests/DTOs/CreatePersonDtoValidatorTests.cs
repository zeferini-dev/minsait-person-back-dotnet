using FluentAssertions;
using Xunit;
using MinsaitPersonBack.DTOs;

namespace MinsaitPersonBack.Tests.DTOs;

public class CreatePersonDtoValidatorTests
{
    private readonly CreatePersonDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new CreatePersonDto { Name = "", Email = "valid@email.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Short()
    {
        var dto = new CreatePersonDto { Name = "", Email = "valid@email.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var dto = new CreatePersonDto { Name = new string('a', 121), Email = "valid@email.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var dto = new CreatePersonDto { Name = "Valid Name", Email = "" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var dto = new CreatePersonDto { Name = "Valid Name", Email = "invalid-email" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Too_Long()
    {
        var dto = new CreatePersonDto { Name = "Valid Name", Email = new string('a', 181) + "@mail.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Be_Valid_When_Data_Is_Correct()
    {
        var dto = new CreatePersonDto { Name = "Ada Lovelace", Email = "ada@example.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}