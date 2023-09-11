using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyShopRazor.Data;
using SurveyShopRazor.Models;

namespace SurveyShopRazor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CreateModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [BindProperty]
        public Category Category { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            _applicationDbContext.Categories.Add(Category);
            _applicationDbContext.SaveChanges();
            TempData["success"] = "Category has been created sucessfully.";
            return RedirectToPage(nameof(Index));
        }
    }
}
