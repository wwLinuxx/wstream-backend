using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class OtpCode : BaseEntity
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = null!;
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}