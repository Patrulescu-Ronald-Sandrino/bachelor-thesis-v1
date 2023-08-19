using System.Net;
using System.Net.Mime;
using System.Text.Json;
using api.DTOs;
using domain.Exceptions;

namespace api.Middlewares;

public class ExceptionHandlerMiddleware
{
    #region non-changing

    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    #endregion

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ErrorResponse response = new() { Message = "Internal server error" };
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (exception is ApplicationException)
        {
            response.Message = exception.Message;
            context.Response.StatusCode = ExceptionToStatusCode[exception.GetType()];
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonSerializerOptions));
    }

    private static readonly Dictionary<Type, int> ExceptionToStatusCode = new()
    {
        { typeof(NotFoundException), StatusCodes.Status404NotFound },
        { typeof(UnauthorizedException), StatusCodes.Status401Unauthorized },
        { typeof(ConflictException), StatusCodes.Status409Conflict }
    };
}