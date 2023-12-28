using BulkyWebRazor_Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWebRazor_Temp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
            
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "War_Razor", DisplayOrder = 2 },
                new Category { Id = 2, Name = "Thriller_Razor", DisplayOrder = 5 },
                new Category { Id = 3, Name = "Horror_Razor", DisplayOrder = 8 }
                );
        }
    }
}

