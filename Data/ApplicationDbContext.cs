using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Villa> Villas { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Luxury villa with a private swimming pool",
                    ImageUrl = "https://bunny-wp-pullzone-dt0gklpcc4.b-cdn.net/wp-content/uploads/2025/08/Evergreen-Escape-Villa-6bhk-scaled-1-1.webp",
                    Occupancy = 4,
                    Rate = 200,
                    Sqft = 500,
                    Amenity = "Swimming Pool",
                    CreatedDate = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Details = "Stunning villa with a sea view.",
                    ImageUrl = "https://media.vrbo.com/lodging/34000000/33580000/33577000/33576964/1fb125ab.jpg?impolicy=resizecrop&rw=1200&ra=fit",
                    Occupancy = 4,
                    Rate = 300,
                    Sqft = 550,
                    Amenity = "Sea view",
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
