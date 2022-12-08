using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicVillaAPI.Models.Dtos
{
    public class VillaNumberCreateDTO
    {
        [NotNull]
        [Key]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
