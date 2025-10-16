using System.ComponentModel.DataAnnotations;
using UzTube.Application.Models;
using UzTube.Attributes;

namespace UzTube.Models.DTO;

public class UpdateUserProfileModel
{
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
    [NotZero]
    public Guid CountryId { get; set; }
}

public class UpdateUserProfileResponseModel : BaseResponseModel { }