using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.User;

namespace FantasyFights.API.Middleware
{
    public class RegistrationDataCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public RegistrationDataCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Equals("/api/authentication/sign-up"))
            {
                var requestBody = httpContext.Request.Body;
                using (StreamReader streamReader = new(requestBody))
                {
                    var requestBodyString = await streamReader.ReadToEndAsync();
                    try
                    {
                        var requestBodyJson = JsonSerializer.Deserialize<UserRegistrationRequestDto>(requestBodyString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (requestBodyJson is null || requestBodyJson.Username is null || requestBodyJson.Password is null)
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                            var responseBody = JsonSerializer.Serialize(new { message = "Username and password are required fields." });
                            httpContext.Response.ContentType = "application/json; charset=UTF-8";
                            await httpContext.Response.WriteAsync(responseBody);
                            return;
                        }
                        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBodyString));
                    }
                    catch (Exception)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        var responseBody = JsonSerializer.Serialize(new { message = "Required request body format is JSON." });
                        httpContext.Response.ContentType = "application/json; charset=UTF-8";
                        await httpContext.Response.WriteAsync(responseBody);
                        return;
                    }
                }
                await _next(httpContext);
            }
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