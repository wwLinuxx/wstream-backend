using UzTube.Core.Common;

namespace UzTube.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }

    public ICollection<PostCategory> PostCategories { get; set; }
}
