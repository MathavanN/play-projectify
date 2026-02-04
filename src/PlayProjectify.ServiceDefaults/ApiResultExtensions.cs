using Microsoft.AspNetCore.Http;

namespace PlayProjectify.ServiceDefaults;

public static class ApiResultExtensions
{
    public static IResult ToApiResult<T>(this ProjectifyServiceResult<T> result, string? createdRouteName = null, Func<T, object>? routeValuesFunc = null)
    {
        if (result.IsSuccess)
        {
            // Return 201 Created if route info provided
            if (!string.IsNullOrEmpty(createdRouteName) && routeValuesFunc != null)
            {
                var routeValues = routeValuesFunc(result.Data!); // safe access to Data
                return TypedResults.CreatedAtRoute(result, createdRouteName, routeValues);
            }

            // Otherwise return 200 OK with the data
            return TypedResults.Ok(result);
        }

        return MapProblem(result);
    }

    private static IResult MapProblem(ProjectifyServiceResult result)
    {
        return result.Error?.Status switch
        {
            StatusCodes.Status400BadRequest => TypedResults.BadRequest(result),
            StatusCodes.Status401Unauthorized => TypedResults.Unauthorized(),
            StatusCodes.Status404NotFound => TypedResults.NotFound(result),
            StatusCodes.Status409Conflict => TypedResults.Conflict(result),
            _ => TypedResults.InternalServerError(result)// 500+
        };
    }
}