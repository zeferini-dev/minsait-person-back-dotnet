using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Xunit;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Filters;

namespace MinsaitPersonBack.Tests.Filters;

public class FluentValidationActionFilterTests
{
    [Fact]
    public void OnActionExecuting_NullArgument_DoesNothing()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new ControllerActionDescriptor()
        };

        var actionArguments = new Dictionary<string, object?> { { "dto", null } };
        var context = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            actionArguments,
            controller: null
        );

        var filter = new FluentValidationActionFilter();

        // Act
        filter.OnActionExecuting(context);

        // Assert
        context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuted_DoesNothing()
    {
        // Arrange
        var actionContext = new ActionContext
        {
            HttpContext = new DefaultHttpContext(),
            RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
            ActionDescriptor = new ControllerActionDescriptor()
        };
        var context = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            controller: null
        );
        var filter = new FluentValidationActionFilter();

        // Act & Assert
        filter.OnActionExecuted(context);
    }

    [Fact]
    public void FluentValidationActionFilter_IsActionFilter()
    {
        var filter = new FluentValidationActionFilter();
        
        filter.Should().BeAssignableTo<IActionFilter>();
    }

    [Fact]
    public void CreatePersonDto_Validation_Works()
    {
        var validator = new CreatePersonDtoValidator();
        var validDto = new CreatePersonDto { Name = "Ada", Email = "ada@example.com" };
        
        var result = validator.Validate(validDto);
        
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreatePersonDto_Validation_FailsOnInvalidData()
    {
        var validator = new CreatePersonDtoValidator();
        var invalidDto = new CreatePersonDto { Name = "", Email = "invalid" };
        
        var result = validator.Validate(invalidDto);
        
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdatePersonDtoValidator_Works()
    {
        var validator = new UpdatePersonDtoValidator();
        var validDto = new UpdatePersonDto { Name = "Ada", Email = "ada@example.com" };
        
        var result = validator.Validate(validDto);
        
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void UpdatePersonDtoValidator_AllowsNullFields()
    {
        var validator = new UpdatePersonDtoValidator();
        var dtoWithNulls = new UpdatePersonDto { Name = null, Email = null };
        
        var result = validator.Validate(dtoWithNulls);
        
        result.IsValid.Should().BeTrue();
    }
}