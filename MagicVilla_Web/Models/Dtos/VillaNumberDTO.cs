using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicVilla_Web.Models.Dtos
{
    public class VillaNumberDTO
    {
        [Required]
        [NotNull]
        [Key]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
