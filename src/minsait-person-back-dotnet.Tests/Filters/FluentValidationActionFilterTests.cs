using System.Text.Json;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Filters;

namespace MinsaitPersonBack.Tests.Filters;

public class FluentValidationActionFilterTests
{
    [Fact]
    public void OnActionExecuting_ShouldIgnoreNullArguments()
    {
        var context = CreateContext(
            arguments: new Dictionary<string, object?> { { "dto", null } },
            registerValidator: false);
        var filter = new FluentValidationActionFilter();

        filter.OnActionExecuting(context);

        context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuting_ShouldIgnoreArgumentsWithoutRegisteredValidator()
    {
        var context = CreateContext(
            arguments: new Dictionary<string, object?> { { "dto", new CreatePersonDto() } },
            registerValidator: false);
        var filter = new FluentValidationActionFilter();

        filter.OnActionExecuting(context);

        context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuting_ShouldContinue_WhenValidationSucceeds()
    {
        var context = CreateContext(
            arguments: new Dictionary<string, object?> { { "dto", new CreatePersonDto { Name = "Ada", Email = "ada@example.com" } } },
            registerValidator: true);
        var filter = new FluentValidationActionFilter();

        filter.OnActionExecuting(context);

        context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuting_ShouldReturnBadRequest_WhenValidationFails()
    {
        var context = CreateContext(
            arguments: new Dictionary<string, object?> { { "dto", new CreatePersonDto { Name = "", Email = "invalid" } } },
            registerValidator: true);
        var filter = new FluentValidationActionFilter();

        filter.OnActionExecuting(context);

        var badRequest = context.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var payload = JsonSerializer.Serialize(badRequest.Value);
        payload.Should().Contain("errors");
        payload.Should().Contain("Name");
        payload.Should().Contain("Email");
    }

    [Fact]
    public void OnActionExecuted_ShouldDoNothing()
    {
        var context = new ActionExecutedContext(
            new ActionContext(new DefaultHttpContext(), new RouteData(), new ControllerActionDescriptor()),
            new List<IFilterMetadata>(),
            new object());

        var filter = new FluentValidationActionFilter();

        var action = () => filter.OnActionExecuted(context);

        action.Should().NotThrow();
    }

    private static ActionExecutingContext CreateContext(
        Dictionary<string, object?> arguments,
        bool registerValidator)
    {
        var services = new ServiceCollection();
        if (registerValidator)
            services.AddScoped<IValidator<CreatePersonDto>, CreatePersonDtoValidator>();

        var actionContext = new ActionContext(
            new DefaultHttpContext { RequestServices = services.BuildServiceProvider() },
            new RouteData(),
            new ControllerActionDescriptor());

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            arguments,
            new object());
    }
}
