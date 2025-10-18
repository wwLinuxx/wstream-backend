using System.ComponentModel.DataAnnotations;

namespace UzTube.Models.DTO;

public class LoginUserModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class LoginResponseModel
{
    public string Email { get; set; }

    public string Token { get; set; }
}
