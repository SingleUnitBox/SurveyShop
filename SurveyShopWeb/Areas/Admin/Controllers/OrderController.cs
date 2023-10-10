using Microsoft.AspNetCore.Mvc;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShop.Models.ViewModels;
using SurveyShop.Utility;
using System.Security.Claims;

namespace SurveyShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId) 
        {
            OrderViewModel = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(x => x.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderViewModel);
        }

        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            IEnumerable<OrderHeader> orders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orders = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == userId,
                    includeProperties: "ApplicationUser");
            }
            
            return Json(new { data = orders });
        }
        #endregion
    }
}
