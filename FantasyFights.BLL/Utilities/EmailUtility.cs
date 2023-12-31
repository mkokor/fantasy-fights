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
                // Use email verification API for complete verification.
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
                throw new Exception("An error occurred while trying to send email message.");
            }
        }

        // This method is customized for this application.
        public static EmailConfiguration ConfigurateEmailData(List<Recipient> recipients, string subject, string body)
        {
            return new EmailConfiguration
            {
                Host = EnvironmentUtility.GetEnvironmentVariable("EMAIL_HOST"),
                Port = int.Parse(EnvironmentUtility.GetEnvironmentVariable("EMAIL_PORT")),
                Sender = new Sender
                {
                    Address = EnvironmentUtility.GetEnvironmentVariable("EMAIL_ADDRESS"),
                    Password = EnvironmentUtility.GetEnvironmentVariable("EMAIL_PASSWORD")
                },
                Recipients = recipients,
                Subject = subject,
                Body = body
            };
        }

        public static string GenerateEmailConfirmationCode()
        {
            var randomNumberGenerator = new Random();
            return $"{randomNumberGenerator.Next(100000, 999999)}";
        }
    }
}