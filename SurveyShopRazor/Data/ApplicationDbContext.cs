using Microsoft.EntityFrameworkCore;
using SurveyShopRazor.Models;

namespace SurveyShopRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Ce1", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Ce2", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Ce3", DisplayOrder = 3 },
            };
            modelBuilder.Entity<Category>().HasData(categories);
        }
    }
}
