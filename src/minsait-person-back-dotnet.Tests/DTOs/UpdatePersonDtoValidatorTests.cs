using FluentAssertions;
using Xunit;
using MinsaitPersonBack.DTOs;

namespace MinsaitPersonBack.Tests.DTOs;

public class UpdatePersonDtoValidatorTests
{
    private readonly UpdatePersonDtoValidator _validator = new();

    [Fact]
    public void Should_Be_Valid_When_All_Properties_Are_Null()
    {
        var dto = new UpdatePersonDto { Name = null, Email = null };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var dto = new UpdatePersonDto { Name = new string('a', 121), Email = null };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Be_Valid_When_Name_At_Max_Length()
    {
        var dto = new UpdatePersonDto { Name = new string('a', 120), Email = null };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Valid_When_Email_Is_Null()
    {
        var dto = new UpdatePersonDto { Name = "Valid Name", Email = null };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Invalid_Format()
    {
        var dto = new UpdatePersonDto { Name = "Valid Name", Email = "invalid-email" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Too_Long()
    {
        var dto = new UpdatePersonDto { Name = "Valid Name", Email = new string('a', 181) + "@mail.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Be_Valid_When_Email_At_Max_Length_And_Valid()
    {
        var dto = new UpdatePersonDto { Name = "Valid Name", Email = new string('a', 170) + "@mail.com" };
        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }
}