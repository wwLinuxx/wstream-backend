using System.ComponentModel.DataAnnotations;
using UzTube.Application.Models;

namespace UzTube.Models.DTO;

public class UpdateUserPasswordModel
{
    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8,
        ErrorMessage = "OldPassword most be minimum length 8 maximum 30")]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8,
        ErrorMessage = "NewPassword most be minimum length 8 maximum 30")]
    public string NewPassword { get; set; }

    [Required]
    [StringLength(maximumLength: 30, MinimumLength = 8,
        ErrorMessage = "ConfirmPassword most be minimum length 8 maximum 30")]
    public string ConfirmPassword { get; set; }
}

public class UpdateUserPasswordResponseModel : BaseResponseModel { }