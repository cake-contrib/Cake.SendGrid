__Sending an email to a single recipient:__

```csharp
#addin Cake.SendGrid&version=0.8.2&loaddependencies=true

var sendGridApiKey = EnvironmentVariable("SENDGRID_API_KEY");

Task("SendEmail")
    .Does(() =>
{
    try
    {
        var result = SendGrid.SendEmail(
                senderName: "Bob Smith", 
                senderAddress: "bob@example.com",
                recipientName: "Jane Doe",
                recipientAddress: "jane@example.com",
                subject: "This is a test",
                htmlContent: "<html><body>This is a test</body></html>",
                textContent: "This is a test",
                attachments: null,
                settings: new SendGridSettings { ApiKey = sendGridApiKey }
        );

        if (result.Ok)
        {
            Information("Email successfully sent");
        }
        else
        {
            Error("Failed to send email: {0}", result.Error);
        }
    }
    catch(Exception ex)
    {
        Error("{0}", ex);
    }
});
```

__Sending an email to multiple recipients:__

```csharp
#addin Cake.SendGrid&version=0.8.1&loaddependencies=true

var sendGridApiKey = EnvironmentVariable("SENDGRID_API_KEY");

Task("SendEmail")
    .Does(() =>
{
    try
    {
        var result = SendGrid.SendEmail(
                senderName: "Bob Smith", 
                senderAddress: "bob@example.com",
                recipients: new[]
                {
                    new Cake.Email.Common.MailAddress("jane@example.com", "Jane Doe"),
                    new Cake.Email.Common.MailAddress("john@example.com", "John Smith")
                },
                subject: "This is a test",
                htmlContent: "<html><body>This is a test</body></html>",
                textContent: "This is a test",
                attachments: null,
                settings: new SendGridSettings { ApiKey = sendGridApiKey }
        );

        if (result.Ok)
        {
            Information("Email successfully sent");
        }
        else
        {
            Error("Failed to send email: {0}", result.Error);
        }
    }
    catch(Exception ex)
    {
        Error("{0}", ex);
    }
});
```
