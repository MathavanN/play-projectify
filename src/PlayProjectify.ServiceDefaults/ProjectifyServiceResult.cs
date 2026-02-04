using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PlayProjectify.ServiceDefaults;

public record ProjectifyServiceResult
{
    public bool IsSuccess { get; }
    public ProblemDetails? Error { get; }

    protected ProjectifyServiceResult(bool isSuccess, ProblemDetails? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static ProjectifyServiceResult Success() => new(true, null);
    public static ProjectifyServiceResult Failure(ProblemDetails error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

    public static implicit operator ProjectifyServiceResult(ProblemDetails error) => Failure(error);
}

public record ProjectifyServiceResult<T> : ProjectifyServiceResult
{
    public T? Data { get; }

    private ProjectifyServiceResult(T data) : base(true, null) => Data = data;
    private ProjectifyServiceResult(ProblemDetails error) : base(false, error) { }

    public static implicit operator ProjectifyServiceResult<T>(T data) => new(data);

    public static implicit operator ProjectifyServiceResult<T>(ProblemDetails error) => new(error);
    public static ProjectifyServiceResult<T> NotFound(string detail) => CreateProblemDetails(HttpStatusCode.NotFound, detail);

    private static ProblemDetails CreateProblemDetails(HttpStatusCode statusCode, string error) =>
        new()
        {
            Status = (int)statusCode,
            Title = statusCode.ToString(),
            Detail = error,
            Type = MapToTypeUri(statusCode)
        };

    private static string MapToTypeUri(HttpStatusCode statusCode) =>
    statusCode switch
    {
        HttpStatusCode.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        HttpStatusCode.BadRequest => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        HttpStatusCode.Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.8",
        HttpStatusCode.Unauthorized => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
        _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1" // 500
    };
}