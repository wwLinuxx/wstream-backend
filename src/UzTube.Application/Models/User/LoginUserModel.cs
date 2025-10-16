using System.ComponentModel.DataAnnotations;

namespace UzTube.Models.DTO;

public class LoginUserModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Email address required")]
    [StringLength(maximumLength: 30, MinimumLength = 8, 
        ErrorMessage = "Email most be minimum length 8 maximum 30")]
    public string Email { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8, 
        ErrorMessage = "Password most be minimum length 8 maximum 30")]
    public string Password { get; set; }
}

public class LoginResponseModel
{
    public string Email { get; set; }

    public string Token { get; set; }
}
