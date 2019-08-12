using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace RussianСertification.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //var emailMessage = new MimeMessage();

            //emailMessage.From.Add(new MailboxAddress("Администрация сайта", "noreply14@bk.ru"));
            //emailMessage.To.Add(new MailboxAddress("", email));
            //emailMessage.Subject = subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            //{
            //    Text = message
            //};

            //using (var client = new SmtpClient())
            //{

            //    await client.SendAsync("smtp.mail.ru", 465, true,SecureSocketOptions.Auto);
            //    await client.AuthenticateAsync("noreply14@bk.ru", "[eqdyjc85");
            //    // await client.
            //    await client.SendAsync(emailMessage);

            //    await client.DisconnectAsync(true);
            //}

            var l_mail = new System.Net.Mail.MailMessage("noreply14@bk.ru", email);
            l_mail.Subject = "Активация учетной записи";
            l_mail.Body = message;
            l_mail.IsBodyHtml = true;

            using (var smtp = new System.Net.Mail.SmtpClient())
            {
                smtp.Host = "smtp.mail.ru";
                smtp.EnableSsl = true;
                var NetworkCred = new System.Net.NetworkCredential("noreply14@bk.ru", "[eqdyjc85");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                await smtp.SendMailAsync(l_mail);
                smtp.SendAsyncCancel();
            }
        }
    }
}
