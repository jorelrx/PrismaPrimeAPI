using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrismaPrimeInvest.Application.Responses;
using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;
using Newtonsoft.Json.Serialization;

namespace PrismaPrimeInvest.Application.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ApiResponse<object> response = exception switch
        {
            ArgumentNullException _ => CreateErrorResponse(context, HttpStatusCode.BadRequest, "A required parameter was null.", exception.Message),
            UnauthorizedAccessException _ => CreateErrorResponse(context, HttpStatusCode.Unauthorized, "Unauthorized access.", exception.Message),
            SecurityTokenException => CreateErrorResponse(context, HttpStatusCode.Unauthorized, "Invalid token.", exception.Message),
            AuthenticationException => CreateErrorResponse(context, HttpStatusCode.Unauthorized, "Authentication failed.", exception.Message),
            ValidationException validationException => CreateValidationErrorResponse(context, validationException),
            CustomIdentityException identityException => CreateIdentityErrorResponse(context, identityException.IdentityResult),
            KeyNotFoundException _ => CreateErrorResponse(context, HttpStatusCode.NotFound, "Resource not found.", exception.Message),
            ApplicationException _ => CreateErrorResponse(context, HttpStatusCode.BadRequest, "An application error occurred.", exception.Message),
            InvalidOperationException _ => CreateErrorResponse(context, HttpStatusCode.BadRequest, "An invalid operation occurred.", exception.Message),
            NotSupportedException _ => CreateErrorResponse(context, HttpStatusCode.BadRequest, "An unsupported operation occurred.", exception.Message),
            TimeoutException _ => CreateErrorResponse(context, HttpStatusCode.RequestTimeout, "A timeout occurred.", exception.Message),
            _ => CreateErrorResponse(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.", exception.Message),
        };
        
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }));
    }

    private static ApiResponse<object> CreateErrorResponse(HttpContext context, HttpStatusCode statusCode, string message, string details)
    {
        context.Response.StatusCode = (int)statusCode;
        return new ApiResponse<object>
        {
            Id = Guid.NewGuid(),
            Status = statusCode,
            Message = message,
            Response = details
        };
    }

    private static ApiResponse<object> CreateValidationErrorResponse(HttpContext context, ValidationException validationException)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return new ApiResponse<object>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.BadRequest,
            Message = "Validation error.",
            Response = new
            {
                ValidationErrors = errors
            }
        };
    }

    public static ApiResponse<object> CreateIdentityErrorResponse(HttpContext context, IdentityResult result)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = result.Errors
            .Select(e => new { Code = e.Code, Description = e.Description })
            .ToList();

        return new ApiResponse<object>
        {
            Id = Guid.NewGuid(),
            Status = HttpStatusCode.BadRequest,
            Message = "User creation failed due to validation errors.",
            Response = new
            {
                IdentityErrors = errors
            }
        };
    }
}
