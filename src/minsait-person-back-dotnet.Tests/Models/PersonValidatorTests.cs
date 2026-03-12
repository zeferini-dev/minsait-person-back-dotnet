using System;
using FluentAssertions;
using Xunit;
using MinsaitPersonBack.Models;

namespace MinsaitPersonBack.Tests.Models;

public class PersonValidatorTests
{
    private readonly PersonValidator _validator = new();

    [Fact]
    public void Should_Be_Valid_When_All_Properties_Are_Valid()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = _validator.Validate(person);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "",
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = _validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = new string('a', 121),
            Email = "ada@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = _validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = "",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = _validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = "invalid-email",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = _validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Too_Long()
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Ada Lovelace",
            Email = new string('a', 181) + "@mail.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = _validator.Validate(person);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }
}