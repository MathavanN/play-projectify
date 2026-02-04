using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace PlayProjectify.ServiceDefaults;

public static class ValidationExtensions
{
    public static RouteHandlerBuilder Validate<T>(
        this RouteHandlerBuilder builder)
        => builder.AddEndpointFilter<ValidationFilter<T>>();
}

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();
        if (argument is null)
            return await next(context);

        var validator = context.HttpContext.RequestServices
            .GetService<IValidator<T>>();

        if (validator is null)
            return await next(context);

        var validationResult = await validator.ValidateAsync(argument);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            var detail = string.Join(", ", errors.SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}")));
            var problemDetails = new ValidationProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Validation error",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path,
                Detail = detail,
                Errors = errors
            };
            return TypedResults.BadRequest(ProjectifyServiceResult.Failure(problemDetails));
        }

        return await next(context);
    }
}