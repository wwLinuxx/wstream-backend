using UzTube.Application.Exceptions;
using UzTube.Application.Models;

namespace UzTube.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        logger.LogError(ex.Message);

        int code = StatusCodes.Status500InternalServerError;
        List<string> errors = new List<string> { ex.Message };

        code = ex switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            UnprocessableRequestException => StatusCodes.Status422UnprocessableEntity,
            _ => code
        };

        context.Response.StatusCode = code;

        await context.Response.WriteAsJsonAsync(ApiResult<string>.Failure(errors));
    }
}