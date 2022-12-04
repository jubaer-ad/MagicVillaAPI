using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.DBContext
{
    public class VillaDBContext : DbContext
    {
        public DbSet<Villa> Villas { get; set; }

        public VillaDBContext(DbContextOptions<VillaDBContext> opt)
            : base(opt)
        {
            Console.WriteLine("In VillaDbContext.cs");
        }

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
                },
                new Villa
                {
                    Id = 2,
                    Name = "B Villa",
                    Details = "Test002 details",
                    ImageUrl = "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                    Occupency = 4,
                    Rate = 8,
                    Amenity = "Test002 Amenity",
                    Sqft = 1050,
                    CreatedAt = DateTime.Now
                },
                new Villa
                {
                    Id = 10,
                    Name = "grrrrrrrrrrr",
                    Details = "hrrrrrrrrrrrrrrr",
                    ImageUrl = "https://images.pexels.com/photos/7583935/pexels-photo-7583935.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                    Occupency = 4,
                    Rate = 8,
                    Amenity = "drrrrrrrrrrrrrrrrrrrrrrr",
                    Sqft = 1050,
                    CreatedAt = DateTime.Now
                }
                );
        }
    }
}
