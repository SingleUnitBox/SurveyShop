using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyShopRazor.Data;
using SurveyShopRazor.Models;

namespace SurveyShopRazor.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EditModel(ApplicationDbContext applicationDbContext)
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
            if (ModelState.IsValid)
            {
                _applicationDbContext.Categories.Update(Category);
                _applicationDbContext.SaveChanges();
                TempData["success"] = "Category has been updated sucessfully.";
                return RedirectToPage(nameof(Index));
            }
            return Page();
        }
    }
}
