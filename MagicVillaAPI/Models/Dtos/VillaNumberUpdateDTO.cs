using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicVillaAPI.Models.Dtos
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        [NotNull]
        [MaxLength(4)]
        [Key]
        public int VillaNo { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
