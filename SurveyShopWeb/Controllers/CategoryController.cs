using Microsoft.AspNetCore.Mvc;
using SurveyShopWeb.Data;
using SurveyShopWeb.Models;

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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Categories.Add(category);
                _applicationDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
