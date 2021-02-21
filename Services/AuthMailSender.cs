using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace NovaWebSolution.Services
{
    public class AuthMailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var senderEmail = new MailAddress("Sender email", "Display name");
                var receiverEmail = new MailAddress(toEmail);
                var password = "your email password";
                var sub = subject;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
               
                using (var mess = new MailMessage(senderEmail, receiverEmail) { Subject = subject, Body = body, IsBodyHtml = true })
                {
                    smtp.SendCompleted += (s, e) => {
                        smtp.Dispose();
                        mess.Dispose();
                    };

                    await smtp.SendMailAsync(mess);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}