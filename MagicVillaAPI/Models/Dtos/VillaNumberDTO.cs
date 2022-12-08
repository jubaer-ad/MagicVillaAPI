using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicVillaAPI.Models.Dtos
{
    public class VillaNumberDTO
    {
        [Required]
        [NotNull]
        [Key]
        public int VillaNo { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
