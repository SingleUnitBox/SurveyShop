using Microsoft.EntityFrameworkCore;
using SurveyShop.Models;

namespace SurveyShopWeb.DataAccess.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Total Station", DisplayOrder = 1 },
                new Category { Id = 2, Name = "GNSS", DisplayOrder= 2 },
                new Category { Id = 3, Name = "Controller", DisplayOrder = 3 }   
            };
            modelBuilder.Entity<Category>().HasData(categories);
        }
    }
}
