using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicVillaAPI.Models.Dtos
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [NotNull]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Occupency { get; set; }
        public int Sqft { get; set; }
    }
}
