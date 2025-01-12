using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Utilities.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            string message;
            int statusCode;

            if (e is ValidationException validationException)
            {
                // ValidationException özel olarak ele alınıyor
                message = FormatValidationErrors(validationException);
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (e is SecurityException securityException)
            {
                // SecurityException için özel format
                var responseObject = new
                {
                    Error = "SecurityError",
                    SecurityMessage = securityException.Message,
                    StatusCode = StatusCodes.Status403Forbidden
                };

                message = JsonSerializer.Serialize(responseObject);
                statusCode = StatusCodes.Status403Forbidden;
            }
            else if (e is UnauthorizedAccessException unauthorizedAccessException)
            {
                var responseObject = new
                {
                    Error = "UnauthorizedAccess",
                    Message = unauthorizedAccessException.Message ?? "Yetkiniz yok.",
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                message = JsonSerializer.Serialize(responseObject);
                statusCode = StatusCodes.Status401Unauthorized;
            }
            else if (e is ApplicationException)
            {
                message = e.Message;
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                message = ExceptionMessage.InternalServerError;
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsync(message);
        }

        private string FormatValidationErrors(ValidationException validationException)
        {
            // Validasyon hatalarını JSON formatında döndürüyoruz
            var errors = validationException.Errors
                .Select(error => new
                {
                    Property = error.PropertyName,
                    ErrorMessage = error.ErrorMessage
                });

            return System.Text.Json.JsonSerializer.Serialize(new
            {
                Errors = errors,
                Message = "Validation failed.",
                StatusCode = (int)HttpStatusCode.BadRequest
            });
        }
    }
}