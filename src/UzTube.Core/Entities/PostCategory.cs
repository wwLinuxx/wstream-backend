using UzTube.Core.Common;

namespace UzTube.Entities;

public class PostCategory : BaseEntity
{
    public Guid PostId { get; set; }
    public Post Post { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}
