
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using ProtectiveWearSecurity.Models;
using ProtectiveWearSecurity.Exceptions;

namespace ProtectiveWearSecurity.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Objeto que representa las propiedades del email remitente.
        /// </summary>
        private readonly EmailSettings _emailSettings;
        /// <summary>
        /// Objeto que representa las propuedades de tipo host a usar durante en envio.
        /// </summary>
        private readonly IHostingEnvironment _env;
        /// <summary>
        /// Constructor de la clase que inyecta la configuracion de la cuenta del correo origen y una propiedad del entorno web
        /// ejecutado desde la aplicación.
        /// </summary>
        /// <param name="emailSettings">Objeto de contiene las propiedades del correo remitente</param>
        /// <param name="env">Proporciona información sobre el entorno de alojamiento web en el que se ejecuta una aplicación.</param>
        public EmailSender(IOptions<EmailSettings> emailSettings, IHostingEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }
        /// <summary>
        /// Constructor de la clase vacio.
        /// </summary>
        public EmailSender()
        {
            
        }
        /// <summary>
        /// Método encargado de enviar el email,
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var mimeMessage = new MimeMessage();
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlMessage;
            bodyBuilder.TextBody = "This is some plain text";

            try
            {
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                mimeMessage.To.Add(new MailboxAddress(email));
                mimeMessage.Subject = subject;
                mimeMessage.Body = bodyBuilder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (_env.IsDevelopment())
                    {
                        // The third parameter is useSSL (true if the client should make an SSL-wrapped
                        // connection to the server; otherwise, false).
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, false);
                    }
                    else
                    {
                        await client.ConnectAsync(_emailSettings.MailServer);
                    }

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (HttpException ex)
            {
                throw new HttpException(ex.Messages);
            }
            
        }

        
    }
}
