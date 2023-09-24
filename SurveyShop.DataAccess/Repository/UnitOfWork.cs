using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShopWeb.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Category = new CategoryRepository(_applicationDbContext);
            Product = new ProductRepository(_applicationDbContext);
            Company = new CompanyRepository(_applicationDbContext);      
            ShoppingCart = new ShoppingCartRepository(_applicationDbContext);
            ApplicationUser = new ApplicationUserRepository(_applicationDbContext);
        }
        

        public void Save()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
