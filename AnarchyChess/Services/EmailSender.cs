using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AnarchyChess.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private SmtpClient smtpClient;

        public string? Username => _config.GetSection("AppSettings").GetSection("Mail_Username").Value;
        public string? Password => _config.GetSection("AppSettings").GetSection("Mail_Password").Value;
        public string? Server => _config.GetSection("AppSettings").GetSection("Mail_Server").Value;
        public string? Address => _config.GetSection("AppSettings").GetSection("Mail_Address").Value;
        public int Port => int.Parse(_config.GetSection("AppSettings").GetSection("Mail_Port").Value!);

        public EmailSender(IConfiguration config)
        {
            _config = config;

            ServicePointManager.ServerCertificateValidationCallback = CustomCertificateValidation;

            smtpClient = new SmtpClient(Server)
            {
                Port = Port,
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var body = message;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(Address),
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };
            mailMessage.To.Add(toEmail);

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            await smtpClient.SendMailAsync(mailMessage);
        }

        static bool CustomCertificateValidation(object s, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            /*
            using (var file = File.Create("Certificates/certificate.cert"))
            {
                var cert = certificate.Export(X509ContentType.Cert);
                file.Write(cert, 0, cert.Length);
            }
            */

            X509Certificate actualCertificate = X509Certificate.CreateFromCertFile("Certificates/certificate.cert");
            if (certificate is not null && actualCertificate is not null)
            {
                return certificate.Equals(actualCertificate);
            }

            return false;
        }
    }
}
