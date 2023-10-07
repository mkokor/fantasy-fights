using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyFights.DAL.Other.Email;

namespace FantasyFights.DAL.Other.Email
{
    public class EmailConfiguration
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required Sender Sender { get; set; }
        public required List<Recipient> Recipients { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}