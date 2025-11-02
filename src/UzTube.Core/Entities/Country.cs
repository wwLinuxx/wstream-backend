using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Country : BaseEntity
{
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;

    public Profile UserProfile { get; set; } = null!;
}