using System.ComponentModel.DataAnnotations;
using UzTube.Attributes;

namespace UzTube.Application.Models.Role;

public record RoleUpdateDTO
{
    [Required]
    [NotZero]
    public int RoleId { get; set; }
}
