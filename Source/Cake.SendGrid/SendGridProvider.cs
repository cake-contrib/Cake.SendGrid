using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.SendGrid.Email;
using StrongGrid;
using StrongGrid.Models;

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
        /// <param name="attachments">Attachments to send with the email</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        /// <example>
        /// <code>
        /// using Cake.Email.Common;
        ///
        /// var apiKey = "... your api key ...";
        /// var attachments = new[]
        /// {
        ///     Attachment.FromLocalFile("C:\\temp\\MyFile.txt"),
        ///     Attachment.FromLocalFile("C:\\temp\\MySpreadsheet.xls"),
        ///     Attachment.FromLocalFile("C:\\temp\\MyFile.pdf"),
        /// };
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
        ///         attachments: attachments,
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
        /// </code>
        /// </example>
        [CakeAliasCategory("Email")]
        public SendGridResult SendEmail(string senderName, string senderAddress, string recipientName, string recipientAddress, string subject, string htmlContent, string textContent, IEnumerable<Cake.Email.Common.Attachment> attachments, SendGridSettings settings)
        {
            var recipient = new Cake.Email.Common.MailAddress(recipientAddress, recipientName);
            return SendEmail(senderName, senderAddress, recipient, subject, htmlContent, textContent, attachments, settings);
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
        /// <param name="attachments">Attachments to send with the email</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        /// <example>
        /// <code>
        /// using Cake.Email.Common;
        ///
        /// var apiKey = "... your api key ...";
        /// var attachments = new[]
        /// {
        ///     Attachment.FromLocalFile("C:\\temp\\MyFile.txt"),
        ///     Attachment.FromLocalFile("C:\\temp\\MySpreadsheet.xls"),
        ///     Attachment.FromLocalFile("C:\\temp\\MyFile.pdf"),
        /// };
        /// try
        /// {
        ///     var result = SendGrid.SendEmail(
        ///         senderName: "Bob Smith",
        ///         senderAddress: "bob@example.com",
        ///         recipient: new Cake.Email.Common.MailAddress("jane@example.com", "Jane Doe"),
        ///         subject: "This is a test",
        ///         htmlContent: "<html><body>This is a test</body></html>",
        ///         textContent: "This is a test",
        ///         attachments: attachments,
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
        /// </code>
        /// </example>
        [CakeAliasCategory("Email")]
        public SendGridResult SendEmail(string senderName, string senderAddress, Cake.Email.Common.MailAddress recipient, string subject, string htmlContent, string textContent, IEnumerable<Cake.Email.Common.Attachment> attachments, SendGridSettings settings)
        {
            var recipients = new[] { recipient };
            return SendEmail(senderName, senderAddress, recipients, subject, htmlContent, textContent, attachments, settings);
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
        /// <param name="attachments">Attachments to send with the email</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        /// <example>
        /// <code>
        /// using Cake.Email.Common;
        ///
        /// var apiKey = "... your api key ...";
        /// var attachments = new[]
        /// {
        ///     Attachment.FromLocalFile("C:\\temp\\MyFile.txt"),
        ///     Attachment.FromLocalFile("C:\\temp\\MySpreadsheet.xls"),
        ///     Attachment.FromLocalFile("C:\\temp\\MyFile.pdf"),
        /// };
        /// try
        /// {
        ///     var result = SendGrid.SendEmail(
        ///         senderName: "Bob Smith",
        ///         senderAddress: "bob@example.com",
        ///         recipients: new[] {
        ///             new Cake.Email.Common.MailAddress("jane@example.com", "Jane Doe"),
        ///             new Cake.Email.Common.MailAddress("john@example.com", "John Smith")
        ///         },
        ///         subject: "This is a test",
        ///         htmlContent: "<html><body>This is a test</body></html>",
        ///         textContent: "This is a test",
        ///         attachments: attachments,
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
        /// </code>
        /// </example>
        [CakeAliasCategory("Email")]
        public SendGridResult SendEmail(string senderName, string senderAddress, IEnumerable<Cake.Email.Common.MailAddress> recipients, string subject, string htmlContent, string textContent, IEnumerable<Cake.Email.Common.Attachment> attachments, SendGridSettings settings)
        {
            try
            {
                if (settings == null)
                {
                    throw new ArgumentNullException(nameof(settings));
                }

                if (string.IsNullOrWhiteSpace(senderAddress))
                {
                    throw new ArgumentNullException(nameof(senderAddress), "You must specify the 'from' email address.");
                }

                if (string.IsNullOrWhiteSpace(subject))
                {
                    throw new ArgumentNullException(nameof(subject), "You must specify the subject.");
                }

                if (string.IsNullOrWhiteSpace(htmlContent) && string.IsNullOrEmpty(textContent))
                {
                    throw new ArgumentException("You must specify the HTML content and/or the text content. We can't send an empty email.");
                }

                if (recipients == null || !recipients.Any(r => r != null))
                {
                    throw new CakeException("You must specify at least one recipient");
                }

                var safeRecipients = recipients.Where(r => r != null && !string.IsNullOrEmpty(r.Address));
                if (!safeRecipients.Any())
                {
                    throw new CakeException("None of the recipients you specified have an email address");
                }

                if (attachments == null)
                {
                    attachments = Enumerable.Empty<Cake.Email.Common.Attachment>();
                }

                using (var client = new Client(settings.ApiKey))
                {
                    _context.Verbose("Sending email to {0} via the SendGrid API...", string.Join(", ", safeRecipients.Select(r => r.Address).ToArray()));

                    var from = new StrongGrid.Models.MailAddress(senderAddress, senderName);
                    var personalizations = safeRecipients.Select(r => new MailPersonalization { To = new[] { new StrongGrid.Models.MailAddress(r.Address, r.Name) } }).ToArray();
                    var contents = new[]
                    {
                        new MailContent("text/plain", textContent),
                        new MailContent("text/html", htmlContent),
                    };
                    var trackingSettings = new TrackingSettings
                    {
                        ClickTracking = new ClickTrackingSettings
                        {
                            EnabledInHtmlContent = false,
                            EnabledInTextContent = false,
                        },
                        OpenTracking = new OpenTrackingSettings { Enabled = false },
                        GoogleAnalytics = new GoogleAnalyticsSettings { Enabled = false },
                        SubscriptionTracking = null,
                    };

                    var sendGridAttachments = attachments
                        .Select(a =>
                        {
                            var buffer = (byte[])null;
                            using (var ms = new MemoryStream())
                            {
                                a.ContentStream.CopyTo(ms);
                                buffer = ms.ToArray();
                            }

                            return new StrongGrid.Models.Attachment()
                            {
                                Content = Convert.ToBase64String(buffer),
                                ContentId = a.ContentId,
                                Disposition = string.IsNullOrEmpty(a.ContentId) ? "attachment" : "inline",
                                FileName = a.Name,
                                Type = a.MimeType,
                            };
                        }).ToArray();

                    var messageId = client.Mail.SendAsync(personalizations, subject, contents, from, null, sendGridAttachments, null, null, null, null, null, null, null, null, null, null, trackingSettings).Result;
                    return new SendGridResult(true, messageId, DateTime.UtcNow.ToString("u"), string.Empty);
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                }

                if (settings.ThrowOnFail.HasValue && settings.ThrowOnFail.Value)
                {
                    throw new CakeException(e.Message);
                }
                else
                {
                    return new SendGridResult(false, null, DateTime.UtcNow.ToString("u"), e.Message);
                }
            }
        }
    }
}