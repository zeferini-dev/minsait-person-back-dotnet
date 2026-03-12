using System;
using FluentAssertions;
using Xunit;
using MinsaitPersonBack.Models;
using FluentValidation;

namespace MinsaitPersonBack.Tests.Models;

public class PersonTests
{
    [Fact]
    public void ValidateAndThrow_ShouldNotThrow_WhenPersonIsValid()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Action act = () => person.ValidateAndThrow();

        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateAndThrow_ShouldThrow_WhenNameIsEmpty()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "",
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Action act = () => person.ValidateAndThrow();

        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Name"));
    }

    [Fact]
    public void ValidateAndThrow_ShouldThrow_WhenEmailIsInvalid()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = "invalid-email",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Action act = () => person.ValidateAndThrow();

        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Email"));
    }

    [Fact]
    public void ValidateAndThrow_ShouldThrow_WhenEmailIsEmpty()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = "",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Action act = () => person.ValidateAndThrow();

        act.Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Email"));
    }
}