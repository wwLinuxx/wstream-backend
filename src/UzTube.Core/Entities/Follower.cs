using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Follower : BaseEntity
{
    public Guid FollowerId { get; set; }
    public Guid FollowingId { get; set; }

    public User Follow { get; set; } = null!;
    public User Following { get; set; } = null!;

    public DateTime FollowedOn { get; set; } = DateTime.UtcNow;
}