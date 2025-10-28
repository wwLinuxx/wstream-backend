using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class PostCategory : BaseEntity
{
    public Guid PostId { get; set; }
    public Guid CategoryId { get; set; }

    public Post Post { get; set; } = null!;
    public Category Category { get; set; } = null!;
}