using System.ComponentModel;
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
    public IFormFile ThumbnailFile { get; set; }

    [Required]
    public IFormFile VideoFile { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsPrivate { get; set; }
}
