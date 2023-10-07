using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FantasyFights.DAL.Other.Email;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FantasyFights.BLL.Utilities
{
    public class EmailUtility
    {
        public static bool IsValidEmail(string value)
        {
            try
            {
                _ = new MailAddress(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SendEmail(EmailConfiguration emailConfiguration)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailConfiguration.Sender.Address));
            emailConfiguration.Recipients.ForEach(recipient => email.To.Add(MailboxAddress.Parse(recipient.Address)));
            email.Subject = emailConfiguration.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailConfiguration.Body };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(emailConfiguration.Host, emailConfiguration.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailConfiguration.Sender.Address, emailConfiguration.Sender.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}