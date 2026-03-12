using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace MinsaitPersonBack.Filters;

public class FluentValidationActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;
            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;
            if (validator != null)
            {
                var result = validator.Validate(new ValidationContext<object>(argument));
                if (!result.IsValid)
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    });
                    return;
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
