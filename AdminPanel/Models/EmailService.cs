using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AdminPanel.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMsg = new MimeMessage();
            emailMsg.From.Add(new MailboxAddress("ElmarTest", "elmarbozoevtest@gmail.com"));
            emailMsg.To.Add(new MailboxAddress("", email));
            emailMsg.Subject = subject;
            emailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                /*        await client.ConnectAsync("smpt.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);*/
                await client.AuthenticateAsync("elmarbozoevtest@gmail.com", "uemlycqqcbyushwf");
                await client.SendAsync(emailMsg);
                await client.DisconnectAsync(true);
            }
        }
    }
}
