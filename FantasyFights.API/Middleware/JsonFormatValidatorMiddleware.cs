using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FantasyFights.BLL.DTOs.User;

namespace FantasyFights.API.Middleware
{
    public class JsonFormatValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonFormatValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var requestBody = httpContext.Request.Body;
            Console.WriteLine("-----JSON Format Validator Middleware-----");
            using (StreamReader streamReader = new(requestBody))
            {
                var requestBodyString = await streamReader.ReadToEndAsync();
                Console.Write(requestBodyString);
                httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBodyString));
            }
            Console.WriteLine("------------------------------------------");
            await _next(httpContext);
        }
    }

    public static class JsonFormatValidatorMiddlewareExtension
    {
        public static IApplicationBuilder UseJsonFormatValidator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JsonFormatValidatorMiddleware>();
        }
    }
}