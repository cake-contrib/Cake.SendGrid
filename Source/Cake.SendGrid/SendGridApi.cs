using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Core;
using StrongGrid;
using StrongGrid.Model;

namespace Cake.SendGrid.Email
{
    /// <summary>
    /// The actual worker for sending email via SendGrid
    /// </summary>
    internal static class SendGridApi
    {
        /// <summary>
        /// Sends an email via the SendGrid API, based on the provided settings
        /// </summary>
        /// <param name="context">The Cake Context</param>
        /// <param name="senderName">The name of the person sending the email</param>
        /// <param name="senderAddress">The email address of the person sending the email</param>
        /// <param name="recipients">An enumeration of recipients who will receive the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="htmlContent">The HTML content</param>
        /// <param name="textContent">The text content</param>
        /// <param name="attachments">Attachments to send with the email</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        internal static SendGridResult SendEmail(this ICakeContext context, string senderName, string senderAddress, IEnumerable<System.Net.Mail.MailAddress> recipients, string subject, string htmlContent, string textContent, IEnumerable<System.Net.Mail.AttachmentBase> attachments, SendGridSettings settings)
        {
            if (recipients == null)
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
                attachments = Enumerable.Empty<System.Net.Mail.AttachmentBase>();
            }

            try
            {
                using (var client = new Client(settings.ApiKey))
                {
                    context.Verbose("Sending email to {0} via the SendGrid API...", string.Join(", ", safeRecipients.Select(r => r.Address).ToArray()));

                    var from = new MailAddress(senderAddress, senderName);
                    var personalizations = safeRecipients.Select(r => new MailPersonalization { To = new[] { new MailAddress(r.Address, r.DisplayName) } }).ToArray();
                    var contents = new[]
                    {
                        new MailContent("text/plain", textContent),
                        new MailContent("text/html", htmlContent)
                    };
                    var trackingSettings = new TrackingSettings
                    {
                        ClickTracking = new ClickTrackingSettings
                        {
                            EnabledInHtmlContent = false,
                            EnabledInTextContent = false
                        },
                        OpenTracking = new OpenTrackingSettings { Enabled = false },
                        GoogleAnalytics = new GoogleAnalyticsSettings { Enabled = false },
                        SubscriptionTracking = null
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

                                return new StrongGrid.Model.Attachment()
                                {
                                    Content = Convert.ToBase64String(buffer),
                                    ContentId = a.ContentId,
                                    Disposition = a is System.Net.Mail.LinkedResource ? "inline" : "attachment",
                                    FileName = a.ContentType.Name,
                                    Type = a.ContentType.MediaType
                                };
                            }).ToArray();

                    client.Mail.SendAsync(personalizations, subject, contents, from, null, sendGridAttachments, null, null, null, null, null, null, null, null, null, null, trackingSettings).Wait();
                }

                return new SendGridResult(true, DateTime.UtcNow.ToString("u"), string.Empty);
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
                    return new SendGridResult(false, DateTime.UtcNow.ToString("u"), e.Message);
                }
            }
        }
    }
}