using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class UserCountry : BaseEntity
{
    public string Code { get; set; } = null!;
    public string FullName { get; set; } = null!;

    public UserProfile UserProfile { get; set; } = null!;
}