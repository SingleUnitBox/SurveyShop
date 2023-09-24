using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyShop.DataAccess.Repository.IRepository;
using SurveyShop.Models;
using SurveyShop.Models.ViewModels;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace SurveyShopWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel shoppingCartViewModel = new()
            {
                CartList = _unitOfWork.ShoppingCart
                .GetAll(x => x.ApplicationUserId == userId, includeProperties: "Product")
            };

            foreach (var cart in shoppingCartViewModel.CartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartViewModel.OrderTotal += (cart.Price * cart.Count);
            }

            return View(shoppingCartViewModel);
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
