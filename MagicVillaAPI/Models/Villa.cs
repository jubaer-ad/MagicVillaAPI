using System.ComponentModel.DataAnnotations;

namespace MagicVillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Occupency { get; set; }
        public int Sqft { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
