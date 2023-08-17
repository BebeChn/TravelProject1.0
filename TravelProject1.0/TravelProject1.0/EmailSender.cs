using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TravelProject1._0
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress("fastic55123@gmail.com");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlMessage;

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            //SmtpClient client = new SmtpClient("smtp.live.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("fastic55123@gmail.com", "hylcdxxmkiffwdjc");
            client.EnableSsl = true;
            client.Send(mail);
        }
    }
}
