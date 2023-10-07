using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace FantasyFights.API.Middleware
{
    public class RegistrationDataCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public RegistrationDataCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static async Task SendErrorResponse(HttpContext httpContext, string errorMessage)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            var responseBody = JsonSerializer.Serialize(new { message = errorMessage });
            httpContext.Response.ContentType = "application/json; charset=UTF-8";
            await httpContext.Response.WriteAsync(responseBody);
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Equals("/api/authentication/sign-up"))
            {
                using StreamReader streamReader = new(httpContext.Request.Body);
                var requestBodyString = await streamReader.ReadToEndAsync();
                try
                {
                    var requestBodyJson = JsonSerializer.Deserialize<UserRegistrationRequestDto>(requestBodyString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBodyString));
                }
                catch (Exception)
                {
                    await SendErrorResponse(httpContext, "Provide email, username and password in JSON format.");
                    return;
                }
            }
            await _next(httpContext);
        }
    }

    public static class RegistrationDataCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseRegistrationDataCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RegistrationDataCheckMiddleware>();
        }
    }
}