using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace NovaWebSolution.Services
{
    public class AuthMailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                string companyName = WebConfigurationManager.AppSettings["CompanyName"];
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                var senderEmail = new MailAddress(section.From, companyName);
                var receiverEmail = new MailAddress(toEmail);
                //var password = "punam@123";
                var password = section.Network.Password;
                var sub = subject;

                var smtp = new SmtpClient
                {
                    Host = section.Network.Host,
                    Port = section.Network.Port,
                    EnableSsl = section.Network.EnableSsl,
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}