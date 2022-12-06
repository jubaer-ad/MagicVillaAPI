using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillaAPI.Models
{
    public class VillaNumber : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        public string? SpecialDetails { get; set; }
        public DateTime CreatedAt { get ; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
