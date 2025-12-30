namespace UzTube.Application.Common.Email;

public record EmailAttachment
{
    public byte[] Value { get; private init; } = null!;
    public string Name { get; private init; } = null!;


    public static EmailAttachment Create(byte[] value, string name)
    {
        return new EmailAttachment
        {
            Value = value,
            Name = name
        };
    }
}