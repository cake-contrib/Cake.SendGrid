```csharp
#addin Cake.SendGrid

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
                settings: new SendGridEmailSettings { ApiKey = sendGridApiKey }
        );

        if (result.Ok)
        {
            Information("Email succcessfully sent");
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