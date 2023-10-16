using System.Text;
using System.Text.Json;
using FantasyFights.BLL.DTOs.EmailConfirmation;
using FantasyFights.BLL.DTOs.User;

namespace FantasyFights.API.Middlewares
{
    public class JsonBodyDataCheckMiddleware<T>
    {
        private readonly RequestDelegate _next;
        private readonly string _requestPath;
        private readonly string _errorMessage;

        public JsonBodyDataCheckMiddleware(RequestDelegate next, string requestPath, string errorMessage)
        {
            _next = next;
            _requestPath = requestPath;
            _errorMessage = errorMessage;
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
            if (httpContext.Request.Path.Equals(_requestPath))
            {
                using StreamReader streamReader = new(httpContext.Request.Body);
                var requestBodyString = await streamReader.ReadToEndAsync();
                try
                {
                    var requestBodyJson = JsonSerializer.Deserialize<T>(requestBodyString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBodyString));
                }
                catch (Exception)
                {
                    await SendErrorResponse(httpContext, _errorMessage);
                    return;
                }
            }
            await _next(httpContext);
        }
    }

    public static class JsonBodyDataCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseRegistrationDataCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonBodyDataCheckMiddleware<UserRegistrationRequestDto>>("/api/auth/sign-up", "Provide email, username and password in JSON format.");
        }

        public static IApplicationBuilder UseEmailConfirmationDataCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonBodyDataCheckMiddleware<EmailConfirmationRequestDto>>("/api/auth/email/confirmation", "Provide email and confirmation code in JSON format.");
        }

        public static IApplicationBuilder UseEmailConfirmationCodeRequestDataCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonBodyDataCheckMiddleware<EmailConfirmationCodeRequestDto>>("/api/auth/email/confirmation-code-refresh", "Provide email in JSON format.");
        }
    }
}