using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace PlayProjectify.Core.ApiProblemBuilder;

public sealed class ApiProblemBuilder : IApiProblemBuilder
{
    private readonly ProblemDetailsFactory _factory;
    private readonly IHttpContextAccessor _context;

    public ApiProblemBuilder(ProblemDetailsFactory factory, IHttpContextAccessor context)
    {
        _factory = factory;
        _context = context;
    }

    public ProblemDetails NotFound(string detail)
        => Create(StatusCodes.Status404NotFound, "Not Found", detail);

    public ProblemDetails BadRequest(string detail)
        => Create(StatusCodes.Status400BadRequest, "Bad Request", detail);

    public ProblemDetails Unauthorized(string detail)
        => Create(StatusCodes.Status401Unauthorized, "Unauthorized", detail);

    private ProblemDetails Create(int status, string title, string detail)
    {
        return _factory.CreateProblemDetails(
            _context.HttpContext!,
            statusCode: status,
            title: title,
            detail: detail);
    }
}