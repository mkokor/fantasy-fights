using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
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

        public static void SendEmail()
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("brennan46@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("brennan46@ethereal.email"));
            email.Subject = "Test";
            email.Body = new TextPart(TextFormat.Html) { Text = "This is a test." };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("brennan46@ethereal.email", "em2qJpJyKtzMjHVnr5");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}