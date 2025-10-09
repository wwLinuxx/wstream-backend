using UzTube.Application.Models.Role;

namespace UzTube.Models.DTO;

public record UserGetDTO
{
    public int Id { get; set; }

    public string Email { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public int Age { get; set; }

    public string Country { get; set; }

    public string CreatedAt { get; set; }

    public List<RoleGetDTO> Roles { get; set; }
}
