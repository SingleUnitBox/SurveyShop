using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyShopRazor.Data;
using SurveyShopRazor.Models;

namespace SurveyShopRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public List<Category> Categories { get; set; }

        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void OnGet()
        {
            Categories = _applicationDbContext.Categories.ToList();
        }
    }
}
