using System.ComponentModel.DataAnnotations;
using UzTube.Attributes;

namespace UzTube.Models.DTO;

public record UserRoleUpdateDTO
{
    [Required]
    [NotZero]
    public ISet<int> RoleIds { get; set; }
}
