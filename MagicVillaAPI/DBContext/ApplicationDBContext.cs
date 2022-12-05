using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.DBContext
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Villa> Villas { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt)
            : base(opt) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "A Villa",
                    Details = "Test details",
                    ImageUrl = "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                    Occupency = 2,
                    Rate = 5,
                    Amenity = "Test Amenity",
                    Sqft = 750,
                    CreatedAt = DateTime.Now
                }
                );
        }
    }
}
