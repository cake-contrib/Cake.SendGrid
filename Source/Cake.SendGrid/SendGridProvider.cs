using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.SendGrid.Email;

namespace Cake.SendGrid
{
    /// <summary>
    /// Contains functionality related to SendGrid API
    /// </summary>
    [CakeAliasCategory("SendGrid")]
    public sealed class SendGridProvider
    {
        private readonly ICakeContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SendGridProvider(ICakeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Send an email through SendGrid
        /// </summary>
        /// <param name="senderName">The name of the person sending the email</param>
        /// <param name="senderAddress">The email address of the person sending the email</param>
        /// <param name="recipientName">The name of the person who will receive the email</param>
        /// <param name="recipientAddress">The email address of the person who will recieve the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="htmlContent">The HTML content</param>
        /// <param name="textContent">The text content</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        /// <example>
        /// var apiKey = "... your api key ...";
        /// try
        /// {
        ///     var result = SendGrid.SendEmail(
        ///         senderName: "Bob Smith",
        ///         senderAddress: "bob@example.com",
        ///         recipientName: "Jane Doe",
        ///         recipientAddress: "jane@example.com",
        ///         subject: "This is a test",
        ///         htmlContent: "<html><body>This is a test</body></html>",
        ///         textContent: "This is a test",
        ///         settings: new SendGridEmailSettings { ApiKey = apiKey }
        ///     );
        ///     if (result.Ok)
        ///     {
        ///         Information("Email succcessfully sent");
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send email: {0}", result.Error);
        ///     }
        /// }
        /// catch(Exception ex)
        /// {
        ///     Error("{0}", ex);
        /// }
        /// </example>
        [CakeAliasCategory("Email")]
        public SendGridResult SendEmail(string senderName, string senderAddress, string recipientName, string recipientAddress, string subject, string htmlContent, string textContent, SendGridSettings settings)
        {
            var recipient = new MailAddress(recipientAddress, recipientName);
            return SendEmail(senderName, senderAddress, recipient, subject, htmlContent, textContent, settings);
        }

        /// <summary>
        /// Send an email through SendGrid
        /// </summary>
        /// <param name="senderName">The name of the person sending the email</param>
        /// <param name="senderAddress">The email address of the person sending the email</param>
        /// <param name="recipient">The recipient who will receive the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="htmlContent">The HTML content</param>
        /// <param name="textContent">The text content</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        /// <example>
        /// var apiKey = "... your api key ...";
        /// try
        /// {
        ///     var result = SendGrid.SendEmail(
        ///         senderName: "Bob Smith",
        ///         senderAddress: "bob@example.com",
        ///         recipient: new MailAddress("jane@example.com", "Jane Doe"),
        ///         subject: "This is a test",
        ///         htmlContent: "<html><body>This is a test</body></html>",
        ///         textContent: "This is a test",
        ///         settings: new SendGridEmailSettings { ApiKey = apiKey }
        ///     );
        ///     if (result.Ok)
        ///     {
        ///         Information("Email succcessfully sent");
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send email: {0}", result.Error);
        ///     }
        /// }
        /// catch(Exception ex)
        /// {
        ///     Error("{0}", ex);
        /// }
        /// </example>
        [CakeAliasCategory("Email")]
        public SendGridResult SendEmail(string senderName, string senderAddress, System.Net.Mail.MailAddress recipient, string subject, string htmlContent, string textContent, SendGridSettings settings)
        {
            var recipients = new[] { recipient };
            return SendEmail(senderName, senderAddress, recipients, subject, htmlContent, textContent, settings);
        }

        /// <summary>
        /// Send an email through SendGrid
        /// </summary>
        /// <param name="senderName">The name of the person sending the email</param>
        /// <param name="senderAddress">The email addresses of the person sending the email</param>
        /// <param name="recipients">An enumeration of recipients who will receive the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="htmlContent">The HTML content</param>
        /// <param name="textContent">The text content</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        /// <example>
        /// <code>
        /// var apiKey = "... your api key ...";
        /// try
        /// {
        ///     var result = SendGrid.SendEmail(
        ///         senderName: "Bob Smith",
        ///         senderAddress: "bob@example.com",
        ///         recipients: new[] {
        ///             new MailAddress("jane@example.com", "Jane Doe"),
        ///             new MailAddress("jane@example.com", "Jane Doe")
        ///            },
        ///         subject: "This is a test",
        ///         htmlContent: "<html><body>This is a test</body></html>",
        ///         textContent: "This is a test",
        ///         settings: new CakeMailEmailSettings
        ///         {
        ///         ApiKey = apiKey,
        ///         UserName = userName,
        ///         Password = password
        ///         }
        ///     );
        ///     if (result.Ok)
        ///     {
        ///         Information("Email succcessfully sent");
        ///     }
        ///     else
        ///     {
        ///         Error("Failed to send email: {0}", result.Error);
        ///     }
        /// }
        /// catch(Exception ex)
        /// {
        ///     Error("{0}", ex);
        /// }
        /// </code>
        /// </example>
        [CakeAliasCategory("Email")]
        public SendGridResult SendEmail(string senderName, string senderAddress, IEnumerable<System.Net.Mail.MailAddress> recipients, string subject, string htmlContent, string textContent, SendGridSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(senderAddress))
            {
                throw new ArgumentNullException(nameof(senderAddress), "You must specify the 'from' email address.");
            }

            if (recipients == null || !recipients.Any(r => r != null && !string.IsNullOrEmpty(r.Address)))
            {
                throw new ArgumentNullException(nameof(recipients), "You must specify at least one recipient.");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject), "You must specify the subject.");
            }

            if (string.IsNullOrWhiteSpace(htmlContent) && string.IsNullOrEmpty(textContent))
            {
                throw new ArgumentException("You must specify the HTML content and/or the text content. We can't send an empty email.");
            }

            return _context.SendEmail(senderName, senderAddress, recipients, subject, htmlContent, textContent, settings);
        }
    }
}