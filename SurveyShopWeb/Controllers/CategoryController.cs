using Microsoft.AspNetCore.Mvc;
using SurveyShopWeb.Data;

namespace SurveyShopWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            var categories = _applicationDbContext.Categories.ToList();
            return View(categories);
        }
    }
}
