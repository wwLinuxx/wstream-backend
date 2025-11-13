namespace UzTube.Application.Common.Email;

public record EmailMessage
{
    private EmailMessage() { }
    
    public string ToAddress { get; private init; } = null!;
    public string Body { get; private init; } = null!;
    public string Subject { get; private init; } = null!;

    public List<EmailAttachment> Attachments { get; private init; } = null!;

    public static EmailMessage Create(string toAddress, string body, string subject, List<EmailAttachment>? attachments = null)
    {
        attachments ??= [];
        
        return new EmailMessage
        {
            ToAddress = toAddress,
            Body = body,
            Subject = subject,
            Attachments = attachments
        };
    }

    public static EmailMessage Create(string toAddress, string body, string subject, EmailAttachment attachment)
    {
        return new EmailMessage
        {
            ToAddress = toAddress,
            Body = body,
            Subject = subject,
            Attachments = [attachment]
        };
    }
}