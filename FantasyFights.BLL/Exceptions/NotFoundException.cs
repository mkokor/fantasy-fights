using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FantasyFights.BLL.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public NotFoundException(string? message) : base(message, HttpStatusCode.NotFound) { }
    }
}