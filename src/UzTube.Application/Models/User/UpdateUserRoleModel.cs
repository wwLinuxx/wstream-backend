using System.ComponentModel.DataAnnotations;
using UzTube.Application.Models;
using UzTube.Attributes;

namespace UzTube.Models.DTO;

public class UpdateUserRoleModel
{
    [Required]
    [NotZero]
    public ISet<int> RoleIds { get; set; }
}

public class UpdateUserRoleResponseModel : BaseResponseModel { }
