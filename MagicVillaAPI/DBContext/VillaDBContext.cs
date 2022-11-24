using MagicVillaAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.DBContext
{
    public class VillaDBContext : DbContext
    {
        public VillaDBContext(DbContextOptions<VillaDBContext> options) : base(options) {}

        public DbSet<VillaDTO> villaDTOs => Set<VillaDTO>();
    }
}
