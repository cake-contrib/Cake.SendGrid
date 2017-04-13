using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        internal static SendGridResult SendEmail(this ICakeContext context, string senderName, string senderAddress, IEnumerable<System.Net.Mail.MailAddress> recipients, string subject, string htmlContent, string textContent, SendGridSettings settings)
        {
            try
            {
                using (var client = new Client(settings.ApiKey, proxy))
                {
                    context.Verbose("Sending email to {0} via the SendGrid API...", string.Join(", ", recipients.Select(r => r.Address).ToArray()));

                    var from = new MailAddress(senderAddress, senderName);
                    var to = recipients.Where(r => r != null && !string.IsNullOrEmpty(r.Address)).Select(r => new MailAddress(r.Address, r.DisplayName)).ToArray();

                    client.Mail.SendToMultipleRecipientsAsync(to, from, subject, htmlContent, textContent, false, false).Wait();
                }

                return new SendGridResult(true, DateTime.UtcNow.ToString("u"), string.Empty);
            }
            catch (Exception e)
            {
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