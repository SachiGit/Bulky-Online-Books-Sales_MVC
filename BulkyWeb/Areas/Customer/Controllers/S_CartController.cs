using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
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

        [BindProperty]  //Bind the ShoppingCartViewModel | Data when hit the Submit Btn,So it would not forcefully pass as a parameter from OrderSummaryPOST() Line #79
		public ShoppingCartViewModel ShoppingCartViewModel { get; set; }   //This View Model will be automatically populated

        public S_CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()                                            // Shopping Cart Index Action
        {
            var claimID = (ClaimsIdentity)User.Identity;    //default methods
            var userId = claimID.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartViewModel.ShoppingCartList)  //Order Total Calculating
            {
                cart.Price = GetPriceBaseOn_Qty(cart);      //Price=> ShoppingCart.cs--> [Not Mapped] variable | Only for calculate and display | Not added to the DB col.
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }

        public IActionResult OrderSummary()             //Shopping Cart Order Summary Action
        {
            var claimID = (ClaimsIdentity)User.Identity;    //default methods
            var userId = claimID.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };
            ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartViewModel.OrderHeader.StreeAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.StreeAddress;
            ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;
            
            foreach (var cart in ShoppingCartViewModel.ShoppingCartList)   //Order Total Calculating
            {
                cart.Price = GetPriceBaseOn_Qty(cart);      //Price=> ShoppingCart.cs--> [Not Mapped] variable | Only for calculate and display | Not added to the DB col.
                ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }



		// Order Summary POST Action Method 
		[HttpPost]
        [ActionName("OrderSummary")]
		public IActionResult OrderSummaryPOST()              
		{
			var claimID = (ClaimsIdentity)User.Identity;    
			var userId = claimID.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");    //All data will populate, if data missed, so this line should add...

            ShoppingCartViewModel.OrderHeader.OrderDate = System.DateTime.Now;    //Order Date
            ShoppingCartViewModel.OrderHeader.ApplicationUserId = userId;         // Who is the User (Normal Customer or a Company Customer)

			ApplicationUser applicationUser= _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

			if (applicationUser.CompanyId.GetValueOrDefault() == 0)  //If Company ID = 0 | Normal Customer
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;   //Check the Payments
				ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.StatusPending;            //Check the Order
			}

            else       //Company Customer
            {
                ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;   // OK for delayed payments
                ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.StatusApproved;                 //  Order is Approved
            }

			foreach (var cart in ShoppingCartViewModel.ShoppingCartList)   //Order Total Calculating
			{
				cart.Price = GetPriceBaseOn_Qty(cart);      //Price=> ShoppingCart.cs--> [Not Mapped] variable | Only for calculate and display | Not added to the DB col.
				ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

            _unitOfWork.OrderHeader.Add(ShoppingCartViewModel.OrderHeader);  //Create the Order Header Record
			_unitOfWork.Save();    

            foreach (var odrDetails in ShoppingCartViewModel.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                { 
                    ProductId = odrDetails.ProductId,
                    OrderHeaderId = ShoppingCartViewModel.OrderHeader.Id,
                    Price = odrDetails.Price,
                    Count = odrDetails.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);   //Create the Order Detail Record
				_unitOfWork.Save(); 
			}

			if (applicationUser.CompanyId.GetValueOrDefault() == 0)  
			{
                //Normal Customer | So we have to track the payments
                //Stripe Logic Here
			}
            //return View(ShoppingCartViewModel);   //ToDo Redirecting to a Confirmation Page
            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartViewModel.OrderHeader.Id });
		}

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
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
