using UzTube.Application.Models;

namespace UzTube.Models.DTO;

public class CreateUserModel
{
    public string Email { get; set; }
 
    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public int Age { get; set; }

    public Guid CountryId { get; set; }
}

public class CreateUserResponseModel : BaseResponseModel { }
