using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace inProjects.EmailJury
{
    public class Emailer
    {
        EmailerOptions _options;

        public Emailer(IOptionsMonitor<EmailerOptions> options)
        {
            _options = options.CurrentValue;
            options.OnChange(opts => 
            {
                _options = opts;
            });

        }

        private void ConfigChanged(EmailerOptions options)
        {
            _options = options;
        }

        private MimeMessage CreateMessage(string to, string subject, string content)
        {
            // make the e-mail message
            MimeMessage message = new MimeMessage();
            message.To.Add(new MailboxAddress(to));
            message.From.Add(new MailboxAddress(_options.address));
            message.Subject = subject;
            var builder = new BodyBuilder();
            message.Body = new TextPart("plain"){Text = content};
            builder.HtmlBody = string.Format( content );
            message.Body = builder.ToMessageBody();

            return message;
        }

        public async Task SendMessage(string to, string subject, string content)
        {
            MimeMessage msg = CreateMessage(to, subject, content);

            using (SmtpClient client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s,c,h,e) => true;

				client.Connect(_options.smtp_server, _options.smtp_server_port, false);    // Never use Ssl
				client.Authenticate(_options.username, _options.password);

				await client.SendAsync(msg);

				client.Disconnect(true);
            }
        }
    }
}
