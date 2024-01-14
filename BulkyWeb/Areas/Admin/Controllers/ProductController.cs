using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();   //which repository we rae working on (Product Repo...)
            return View(objProductList);
        }

        public IActionResult Create()     //Default [HttpGet]
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //if (obj.Name == obj.DisplayName.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order Cannot Exactly Match the Product Name!");
            //}

            //if (obj.Name.ToLower() == "test")   //Additional
            //{
            //    ModelState.AddModelError("", "Test is an Invalid Value!");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);  //which repository we rae working on (Product Repo...)
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)             //Default [HttpGet]
        {
            if (id == 0 || id == null)
            {
                NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);                             //Method 1
            //Product? productFromDb1 = _db.Categories.FirstOrDefault(a=>a.Id==id);             //Method 2
            //Product? productFromDb2 = _db.Categories.Where(a=>a.Id==id).FirstOrDefault();     //Method 3
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            //if (obj.Name == obj.DisplayName.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order Cannot Exactly Match the Product Name!");
            //}

            //if (obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "Test is an Invalid Value!");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Edited Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)             //Default [HttpGet]
        {
            if (id == 0 || id == null)
            {
                NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);                         //Method 1
            //Product? productFromDb1 = _db.Categories.FirstOrDefault(a=>a.Id==id);             //Method 2
            //Product? productFromDb2 = _db.Categories.Where(a=>a.Id==id).FirstOrDefault();     //Method 3
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]     //Explicitly defined the End point 'Delete' Action
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
