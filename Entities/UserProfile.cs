namespace UzTube.Entities;

public class UserProfile
{
    public int UserId { get; set; }
    public User User { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public int Age { get; set; }

    public int CountryId { get; set; }
    public UserCountry Country { get; set; }
}
