using System.ComponentModel.DataAnnotations;

namespace UzTube.Models.DTO;

public record PostCreateDTO
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    [StringLength(20)]
    public string Duration { get; set; }

    [Required]
    [StringLength(1000)]
    public string VideoUrl { get; set; }

    [Required]
    public bool IsPrivate { get; set; } = false;
}
