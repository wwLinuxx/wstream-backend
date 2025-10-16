using UzTube.Core.Common;

namespace UzTube.Entities;

public class UserCountry : BaseEntity
{
    public string Name { get; set; }

    public UserProfile UserProfile { get; set; }
}