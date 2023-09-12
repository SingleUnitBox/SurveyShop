using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShopWeb.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        void ICategoryRepository.Update(Category category)
        {
            _applicationDbContext.Categories.Update(category);
        }
    }
}
