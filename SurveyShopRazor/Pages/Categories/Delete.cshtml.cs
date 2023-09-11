using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyShopRazor.Data;
using SurveyShopRazor.Models;
using System.Diagnostics;

namespace SurveyShopRazor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeleteModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [BindProperty]
        public Category? Category { get; set; }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Category = _applicationDbContext.Categories.FirstOrDefault(x => x.Id == id);
            }
        }
        public IActionResult OnPost()
        {
            var categoryFromDb = _applicationDbContext.Categories.FirstOrDefault(x => x.Id == Category.Id);
            if (categoryFromDb is null)
            {
                return NotFound();
            }
            _applicationDbContext.Categories.Remove(categoryFromDb);
            _applicationDbContext.SaveChanges();
            TempData["success"] = "Category has been deleted sucessfully.";
            return RedirectToPage(nameof(Index));
        }
    }
}
