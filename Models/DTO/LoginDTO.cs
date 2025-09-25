using System.ComponentModel.DataAnnotations;

namespace UzTube.Models.DTO;

public record LoginDTO
{
    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Username most be minimum legth 5 maximum 30")]
    public string Email { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Password most be minimum legth 5 maximum 30")]
    public string Password { get; set; }
}
