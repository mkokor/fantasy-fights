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
        private static MimeMessage ConfigureEmail(EmailConfiguration emailConfiguration)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Fantasy Fights", emailConfiguration.Sender.Address));
            emailConfiguration.Recipients.ForEach(recipient => email.To.Add(MailboxAddress.Parse(recipient.Address)));
            email.Subject = emailConfiguration.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailConfiguration.Body };
            return email;
        }

        public static bool IsValid(string value)
        {
            try
            {
                // Use email verification API for complete verification
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
            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtpClient.Connect(emailConfiguration.Host, emailConfiguration.Port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(emailConfiguration.Sender.Address, emailConfiguration.Sender.Password);
                smtpClient.Send(ConfigureEmail(emailConfiguration));
                smtpClient.Disconnect(true);
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong.");
            }
        }
    }
}