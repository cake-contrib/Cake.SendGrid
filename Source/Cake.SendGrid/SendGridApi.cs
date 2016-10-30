using System;
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
        /// <param name="recipientName">The name of the person who will receive the email</param>
        /// <param name="recipientAddress">The email address of the person who will recieve the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="htmlContent">The HTML content</param>
        /// <param name="textContent">The text content</param>
        /// <param name="settings">The settings to be used when sending the email</param>
        /// <returns>An instance of <see cref="SendGridResult"/> indicating success/failure</returns>
        internal static SendGridResult SendEmail(this ICakeContext context, string senderName, string senderAddress, string recipientName, string recipientAddress, string subject, string htmlContent, string textContent, SendGridSettings settings)
        {
            try
            {
                using (var client = new Client(settings.ApiKey))
                {
                    context.Verbose("Sending email to {0} via the SendGrid API...", recipientAddress);

                    var from = new MailAddress(senderAddress, senderName);
                    var to = new MailAddress(recipientAddress, recipientName);

                    client.Mail.SendToSingleRecipientAsync(to, from, subject, htmlContent, textContent).Wait();
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