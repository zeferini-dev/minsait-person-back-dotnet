using System;
using FluentAssertions;
using Xunit;
using MinsaitPersonBack.DTOs;
using FluentValidation;

namespace MinsaitPersonBack.Tests.DTOs;

public class UpdatePersonDtoTests
{
    [Fact]
    public void Name_Too_Long_Should_Throw_ValidationException()
    {
        var dto = new UpdatePersonDto
        {
            Name = new string('a', 121),
            Email = "valid@email.com"
        };

        Action act = () => dto.ValidateAndThrow();

        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Name"));
    }

    [Fact]
    public void Email_Invalid_Format_Should_Throw_ValidationException()
    {
        var dto = new UpdatePersonDto
        {
            Name = "Valid Name",
            Email = "invalid-email"
        };

        Action act = () => dto.ValidateAndThrow();

        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Email"));
    }

    [Fact]
    public void Email_Too_Long_Should_Throw_ValidationException()
    {
        var dto = new UpdatePersonDto
        {
            Name = "Valid Name",
            Email = new string('a', 181) + "@mail.com"
        };

        Action act = () => dto.ValidateAndThrow();

        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Email"));
    }

    [Fact]
    public void Null_Properties_Should_Not_Throw()
    {
        var dto = new UpdatePersonDto
        {
            Name = null,
            Email = null
        };

        Action act = () => dto.ValidateAndThrow();

        act.Should().NotThrow();
    }

    [Fact]
    public void Valid_Properties_Should_Not_Throw()
    {
        var dto = new UpdatePersonDto
        {
            Name = "Ada Lovelace",
            Email = "ada@example.com"
        };

        Action act = () => dto.ValidateAndThrow();

        act.Should().NotThrow();
    }
}