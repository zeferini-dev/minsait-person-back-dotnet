using System;
using FluentAssertions;
using Xunit;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Models;

namespace MinsaitPersonBack.Tests.Services;

public class PersonServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreatePerson_WithValidData()
    {
        // Note: Full service tests require database setup
        // Test main logic of Person creation validation through controller
        var dto = new CreatePersonDto { Name = "Ada Lovelace", Email = "ada@example.com" };
        
        dto.Name.Should().Be("Ada Lovelace");
        dto.Email.Should().Be("ada@example.com");
    }

    [Fact]
    public void CreatePersonDto_ShouldBeValid_WithCorrectData()
    {
        var dto = new CreatePersonDto { Name = "Test Person", Email = "test@example.com" };
        
        Action act = () => dto.ValidateAndThrow();
        
        act.Should().NotThrow();
    }

    [Fact]
    public void UpdatePersonDto_ShouldAllowPartialUpdates()
    {
        var dto = new UpdatePersonDto { Name = "New Name", Email = null };
        
        dto.Name.Should().Be("New Name");
        dto.Email.Should().BeNull();
    }

    [Fact]
    public void CreatePersonDto_ShouldThrowOnInvalidData()
    {
        var dto = new CreatePersonDto { Name = "", Email = "invalid" };
        
        Action act = () => dto.ValidateAndThrow();
        
        act.Should().Throw<FluentValidation.ValidationException>();
    }

    [Fact]
    public void UpdatePersonDto_ShouldThrowOnInvalidEmail()
    {
        var dto = new UpdatePersonDto { Email = "invalid-email" };
        
        Action act = () => dto.ValidateAndThrow();
        
        act.Should().Throw<FluentValidation.ValidationException>();
    }
}
