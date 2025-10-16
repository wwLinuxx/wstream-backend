using UzTube.Core.Common;

namespace UzTube.Entities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public int Age { get; set; }

    public Guid CountryId { get; set; }
    public UserCountry Country { get; set; }
}
