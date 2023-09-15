using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShop.Models.ViewModels;
using System.Drawing;

namespace SurveyShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel
            {
                CategoryList = _unitOfWork.Category.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }),
            };

            if (id == null || id == 0)
            {
                productViewModel.Product = new Product();
            }
            else
            {
                productViewModel.Product = _unitOfWork.Product.Get(x => x.Id == id, includeProperties: "Category");
                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
            }
            return View(productViewModel);
        }
        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product\");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
                }
            

            if (productViewModel.Product.Id == 0)
            {
                _unitOfWork.Product.Add(productViewModel.Product);
                TempData["success"] = "Product has been created successfully.";
            }
            else
            {
                _unitOfWork.Product.Update(productViewModel.Product);
                TempData["success"] = "Product has been updated successfully.";

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
            else
            {
                productViewModel.CategoryList = _unitOfWork.Category.GetAll()
                    .Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    });
                return View(productViewModel);
}
        }

        public IActionResult Delete(int? id)
{
    if (id == null || id == 0)
    {
        return NotFound();
    }
    var product = _unitOfWork.Product.Get(x => x.Id == id);
    return View(product);
}
[HttpPost]
[ActionName("Delete")]
public IActionResult DeletePost(int id)

{
    var productFrtomDb = _unitOfWork.Product.Get(x => x.Id == id);
    if (productFrtomDb == null)
    {
        return NotFound();
    }
    _unitOfWork.Product.Remove(productFrtomDb);
    _unitOfWork.Save();
    TempData["success"] = "Product has been deleted successfully.";
    return RedirectToAction(nameof(Index));
}
    }
}
