using System.ComponentModel.DataAnnotations;

namespace UzTube.Models.DTO;

public record RegisterDTO
{
    [Required]
    [EmailAddress(ErrorMessage = "Email address required")]
    [StringLength(maximumLength: 50, MinimumLength = 8,
        ErrorMessage = "Email most be minimum length 8 maximum 50")]
    public string Email { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8,
        ErrorMessage = "Password most be minimum length 8 maximum 30")]
    public string Password { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 4,
        ErrorMessage = "FirstName most be minimum length 4 maximum 30")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 4,
        ErrorMessage = "LastName most be minimum length 4 maximum 30")]
    public string LastName { get; set; }

    [Required]
    [StringLength(maximumLength: 13, MinimumLength = 13,
        ErrorMessage = "PhoneNumber most be length 13")]
    public string PhoneNumber { get; set; }

    [Required]
    [Range(7, 90, 
        ErrorMessage = "Age most be minimum value 7 maximum 90")]
    public int Age { get; set; }

    [Required]
    public int Country { get; set; }
}
