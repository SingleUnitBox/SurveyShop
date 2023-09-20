using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyShop.Models;

namespace SurveyShopWeb.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Total Station", DisplayOrder = 1 },
                new Category { Id = 2, Name = "GNSS", DisplayOrder= 2 },
                new Category { Id = 3, Name = "Controller", DisplayOrder = 3 }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Trimble S5",
                    Description = "Compact Total Station",
                    BarCode = "12 34 56",
                    Owner = "Trimble Sweden",
                    ListPrice = 8900,
                    Price = 5900,
                    Price50 = 4900,
                    Price100 = 3900,
                    CategoryId = 1,
                    ImageUrl = "",
                },
                new Product
                {
                    Id = 2,
                    Name = "Trimble S6",
                    Description = "Robust Total Station",
                    BarCode = "24 48 96",
                    Owner = "Trimble Sweden",
                    ListPrice = 7900,
                    Price = 6900,
                    Price50 = 5900,
                    Price100 = 4900,
                    CategoryId = 2,
                    ImageUrl = "",
                },
                new Product
                {
                    Id = 3,
                    Name = "Trimble S7",
                    Description = "Heavy Total Station",
                    BarCode = "15 17 19",
                    Owner = "Trimble Sweden",
                    ListPrice = 9900,
                    Price = 8900,
                    Price50 = 7900,
                    Price100 = 6900,
                    CategoryId = 1,
                    ImageUrl = "",
                }
            };
            modelBuilder.Entity<Product>().HasData(products);    
        }
    }
}
