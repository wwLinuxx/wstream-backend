using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UzTube.Application.Models;

namespace UzTube.Models.DTO;

public class UpdatePostModel
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    [Required]
    [StringLength(1000)]
    public string PhotoUrl { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool IsPrivate { get; set; }
}

public class UpdatePostResponseModel : BaseResponseModel { }