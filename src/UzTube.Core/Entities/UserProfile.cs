using UzTube.Core.Common;

namespace UzTube.Core.Entities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int Age { get; set; }
    public Guid CountryId { get; set; }

    public UserCountry Country { get; set; } = null!;
}
