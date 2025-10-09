namespace UzTube.Entities;

public class UserCountry
{
    public int Id { get; set; }

    public string Name { get; set; }

    public UserProfile UserProfile { get; set; }
}