using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Rating : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public int Rate { get; set; }

    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}