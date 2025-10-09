using System.ComponentModel.DataAnnotations;
using UzTube.Attributes;

namespace UzTube.Models.DTO;

public record RoleUpdateDTO
{
    [Required]
    [NotZero]
    public int RoleId { get; set; }
}
