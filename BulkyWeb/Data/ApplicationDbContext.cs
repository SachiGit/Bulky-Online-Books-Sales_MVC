using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)      
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //At Once??
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "War", DisplayName = 2 },
                new Category { Id = 2, Name = "Thriller", DisplayName = 5 },
                new Category { Id = 3, Name = "Horror", DisplayName = 8 }
                );
        }
    }
}
