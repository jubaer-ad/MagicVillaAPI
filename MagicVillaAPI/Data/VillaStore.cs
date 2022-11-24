using MagicVillaAPI.Models.Dtos;

namespace MagicVillaAPI.Data
{
    public class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO{ Id = 1, Name = "A Villa", Occupency = 4, Sqft = 1000},
            new VillaDTO{ Id = 2, Name = "B Villa", Occupency = 6, Sqft = 1150},
            new VillaDTO{ Id = 3, Name = "C Villa", Occupency = 4, Sqft = 950},
            new VillaDTO{ Id = 4, Name = "D Villa", Occupency = 3, Sqft = 850},
            new VillaDTO{ Id = 5, Name = "E Villa", Occupency = 2, Sqft = 750}
        };
    }
}
