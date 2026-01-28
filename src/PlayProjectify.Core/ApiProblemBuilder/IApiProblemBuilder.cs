using Microsoft.AspNetCore.Mvc;

namespace PlayProjectify.Core.ApiProblemBuilder;

public interface IApiProblemBuilder
{
    ProblemDetails NotFound(string detail);
    ProblemDetails BadRequest(string detail);
    ProblemDetails Unauthorized(string detail);
}