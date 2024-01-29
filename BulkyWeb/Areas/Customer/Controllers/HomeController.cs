using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;   //unitOfWork
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;    //injecting unitOfWork
        }

        public IActionResult Index()   //Home Controller Index Action
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details(int productId)   //Showing all information when click the 'Details' Btn 
        {
            ShoppingCart s_Cart = new()       // Getting a ShoppingCart Type Object when Details Btn clicked
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(s_Cart);
        }

        [HttpPost]
        [Authorize]  //If some one is posting he/she should be Authorized | Does not care the Role | Just Logged in to the WebSite!
        public IActionResult Details(ShoppingCart shoppingCart)   //ShoppingCart
        {
            //Get the loggedin user's user id with the help of Helper Method...
            var claimID = (ClaimsIdentity)User.Identity;    ///default methods
            var userId = claimID.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            //If same user same product cart must be update, else we have to create a new cart...
            ShoppingCart cartFromDB = _unitOfWork.ShoppingCart.Get(u=> u.ProductId == shoppingCart.ProductId && 
            u.ApplicationUserId == userId);

            if (cartFromDB!= null)   //Record is available, we should do the Addition
            {               
                cartFromDB.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDB);
            }
            else   //Create a New Cart
            {              
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            _unitOfWork.Save();
            TempData["success"] = "Cart Updated Successfully!";
            //return View();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
