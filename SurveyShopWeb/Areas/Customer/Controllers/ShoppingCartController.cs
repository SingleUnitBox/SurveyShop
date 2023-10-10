using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShop.Models.ViewModels;
using SurveyShop.Utility;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace SurveyShopWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                CartList = _unitOfWork.ShoppingCart
                .GetAll(x => x.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new(),
            };

            foreach (var cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                CartList = _unitOfWork.ShoppingCart
                    .GetAll(x => x.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new(),
            };

            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(x => x.Id == userId);
            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.Street;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.Postcode;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;


            foreach (var cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartViewModel.OrderHeader.ApplicationUserId = userId;
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(x => x.Id == userId);

            ShoppingCartViewModel.CartList = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId,
                includeProperties: "Product");

            foreach (var cart in ShoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Count * cart.Price);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartViewModel.OrderHeader.OrderStatus = SD.Order_Pending;
                ShoppingCartViewModel.OrderHeader.PaymentStatus = SD.Payment_Pending;
            }
            else
            {
                ShoppingCartViewModel.OrderHeader.OrderStatus = SD.Order_Approved;
                ShoppingCartViewModel.OrderHeader.PaymentStatus = SD.Payment_AllowedLatePayment;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartViewModel.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartViewModel.CartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderHeaderId = ShoppingCartViewModel.OrderHeader.Id,
                    ProductId = cart.ProductId,
                    Count = cart.Count,
                    Price = cart.Price,
                };
                
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
          
            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartViewModel.OrderHeader.Id });

        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(x => x.Id == id, includeProperties: "ApplicationUser");

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(x => x.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }
        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            return shoppingCart.Product.Price100;
        }
        public IActionResult Plus(int? shoppingCartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == shoppingCartId);
            if (cartFromDb == null)
            {
                return NotFound();
            }
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int? shoppingCartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == shoppingCartId);
            if (cartFromDb == null)
            {
                return NotFound();
            }
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int? shoppingCartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.Id == shoppingCartId, tracked: true);
            if (cartFromDb == null)
            {
                return NotFound();
            }
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
