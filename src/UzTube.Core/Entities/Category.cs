using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public ICollection<PostCategory> PostCategories { get; set; } = [];
    public ICollection<CategoryTranslate> Translates { get; set; } = [];
}