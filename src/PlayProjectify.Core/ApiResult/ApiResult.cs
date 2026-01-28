using Microsoft.AspNetCore.Mvc;
using PlayProjectify.Core.ApiProblemBuilder;

namespace PlayProjectify.Core.ApiResult;

public record ApiResult
{
    public bool IsSuccess { get; }
    public ProblemDetails? Error { get; }

    protected ApiResult(bool isSuccess, ProblemDetails? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static ApiResult Success() => new(true, null);
    public static ApiResult Failure(ProblemDetails error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

    public static implicit operator ApiResult(ProblemDetails error) => Failure(error);
}

public record ApiResult<T> : ApiResult
{
    public T? Data { get; }

    private ApiResult(T data) : base(true, null) => Data = data;
    private ApiResult(ProblemDetails error) : base(false, error) { }

    public static implicit operator ApiResult<T>(T data) => new(data);

    public static implicit operator ApiResult<T>(ProblemDetails error) => new(error);
    public static ApiResult<T> NotFound(IApiProblemBuilder problems, string detail) => problems.NotFound(detail);

}