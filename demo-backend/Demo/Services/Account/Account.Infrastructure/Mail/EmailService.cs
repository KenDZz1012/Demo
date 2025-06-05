using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Application.Models.Emails;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Account.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendMail(Email email)
        {
            try
            {
                var client = new MailjetClient(_emailSettings.ApiKey, _emailSettings.SecretKey);

                // Create email request
                var request = new MailjetRequest { Resource = Send.Resource }
                    .Property(Send.Messages, new JArray
                    {
                        new JObject
                        {
                            { "From", new JObject { { "Email", _emailSettings.FromAddress }, { "Name", _emailSettings.FromName } } },
                            { "To", new JArray { new JObject { { "Email", email.To }, { "Name", "Recipient Name" } } } },
                            { "Subject", email.Subject },
                            { "TextPart", email.Body },
                            { "HTMLPart", "<h3>Hello, this is a test email!</h3>" }
                        }
                    });

                // Send email
                var response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Email sent successfully!");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.GetData()}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message); // Hoặc log lại
                // Log the exception (not implemented here)
                return false;
            }
        }
    }
}
