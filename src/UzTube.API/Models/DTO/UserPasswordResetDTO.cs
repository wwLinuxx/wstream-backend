using System.ComponentModel.DataAnnotations;

namespace UzTube.Models.DTO;

public record UserPasswordResetDTO
{
    [Required]
    [EmailAddress(ErrorMessage = "Email address required")]
    [StringLength(maximumLength: 50, MinimumLength = 8,
        ErrorMessage = "Email most be minimum length 8 maximum 50")]
    public string Email { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8,
        ErrorMessage = "NewPassword most be minimum length 8 maximum 30")]
    public string NewPassword { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8,
        ErrorMessage = "ConfirmPassword most be minimum length 8 maximum 30")]
    public string ConfirmPassword { get; set; }
}
