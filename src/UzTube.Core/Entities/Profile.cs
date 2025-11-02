using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class Profile : BaseEntity
{
    public Guid? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public int? Age { get; set; }
    public Guid? CountryId { get; set; }

    public User User { get; set; } = null!;
    public Country Country { get; set; } = null!;
}