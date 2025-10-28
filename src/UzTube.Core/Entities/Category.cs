using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<PostCategory> PostCategories { get; set; } = [];
}