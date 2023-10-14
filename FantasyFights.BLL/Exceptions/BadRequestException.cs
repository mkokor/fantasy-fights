using System.Net;

namespace FantasyFights.BLL.Exceptions
{
    public class BadRequestException : HttpResponseException
    {
        public BadRequestException(string? message) : base(message, HttpStatusCode.BadRequest) { }
    }
}