using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class S_CartController : Controller                       //Shopping Cart Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public S_CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()                                            // Shopping Cart Index Action
        {
            var claimID = (ClaimsIdentity)User.Identity;    ///default methods
            var userId = claimID.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBaseOn_Qty(cart);      //Price=> ShoppingCart.cs--> [Not Mapped] variable | Only for calculate and display | Not added to the DB col.
                ShoppingCartViewModel.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }

        public IActionResult Add(int cartId)             // Cart Plus (+) Btn
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDB.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDB);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Reduce(int cartId)           // Cart Minus (-) Btn
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDB.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDB);
            }
            else
            {
                cartFromDB.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)           // Cart Delete Btn
        {
            var cartFromDB = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDB);
            _unitOfWork.Save();
            TempData["success"] = "Item Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBaseOn_Qty(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)    //1-50
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100) //1-99
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;  // > 99
                }
            }
        }
    }
}
