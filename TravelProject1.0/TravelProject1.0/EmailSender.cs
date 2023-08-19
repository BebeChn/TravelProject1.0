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
                mail.From = new MailAddress("allen955103@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = htmlMessage;

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                //SmtpClient client = new SmtpClient("smtp.live.com");
                //SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
                client.Port = 587;
                //            client.Credentials = new NetworkCredential("aiguymonster9@gmail.com", "pnitcycbzlpkrgrs");
                client.Credentials = new NetworkCredential("allen955103@gmail.com", "bgcbdgblcslutury");
                client.EnableSsl = true;
                client.Send(mail);
            }
			

				
        
    }
}
