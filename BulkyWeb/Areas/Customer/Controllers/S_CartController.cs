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

        public IActionResult Index()
        {
            var claimID = (ClaimsIdentity)User.Identity;    ///default methods
            var userId = claimID.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product")
            };

            foreach(var cart in ShoppingCartViewModel.ShoppingCartList)
            { 
                double price = GetPriceBaseOn_Qty(cart);
                ShoppingCartViewModel.OrderTotal += (price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }

        private double GetPriceBaseOn_Qty(ShoppingCart shoppingCart) 
        {
            if (shoppingCart.Count <=50)    //1-50
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
