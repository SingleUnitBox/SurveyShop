using Microsoft.AspNetCore.Mvc;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;

namespace SurveyShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var companies = _unitOfWork.Company.GetAll();
            return View(companies);
        }
        public IActionResult Upsert(int? id)
        {
            Company company;
            if (id == 0 || id == null)
            {
                company = new Company();
            }
            else
            {
                company = _unitOfWork.Company.Get(x => x.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company has been created successfully.";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company has been updated successfully.";
                }               
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region API CALLS
        public  IActionResult GetAll()
        {
            var companies = _unitOfWork.Company.GetAll();
            return Json(new { data = companies });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyFromDb = _unitOfWork.Company.Get(x => x.Id == id);
            if (companyFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.Company.Remove(companyFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company has been deleted successfully." });
        }
        #endregion
    }
}
