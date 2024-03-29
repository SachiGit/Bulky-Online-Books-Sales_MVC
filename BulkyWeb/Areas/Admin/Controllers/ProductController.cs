﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = StaticDetails.Role_Admin)]  //Globally //Access All Action Methods If Admin Role LoggedIn. Individually Can Be Allpied Also
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();   //which repository we are working on (Product Repo...)
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem    //Not in Instructor's video
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)     //Default [HttpGet]   //Create removed and Update + Insert added
        {
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductViewModel productViewModel = new ProductViewModel() //ProductViewModel productViewModel = new();
            { 
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            Product = new Product()
            };

            if (id == null || id == 0)
            {
                //Create
                return View(productViewModel);
            }
            else
            { 
                //Update
                productViewModel.Product = _unitOfWork.Product.Get(u=>u.Id==id);
                return View(productViewModel);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? file)
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
                string wwwRootPath = _webHostEnvironment.WebRootPath;   //check and saving the image path with Guid
                if (file != null)
                { 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productViewModel.Product.ImageURL))   //Check a new Image URL comming or not
                    {
                        
                        var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageURL.TrimStart('\\'));
                        //Delete the Old Image
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create)) 
                    { 
                        file.CopyTo(fileStream);    
                    }
                    productViewModel.Product.ImageURL = @"\images\product\" + fileName;   //save in images->product
                }

                if (productViewModel.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productViewModel.Product);
                }

                else 
                {
                    _unitOfWork.Product.Update(productViewModel.Product);
                }

                //_unitOfWork.Product.Add(productViewModel.Product);  //which repository we are working on (Product Repo...)
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }

            else 
            {
                productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productViewModel);
            }
            
        }

        /*public IActionResult Edit(int? id)             //Default [HttpGet]
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
        }*/


        //public IActionResult Delete(int? id)             //Default [HttpGet]  //Removed due to repeating on 'public IActionResult Delete(int? id)'  //Line #207
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);                         //Method 1
        //    //Product? productFromDb1 = _db.Categories.FirstOrDefault(a=>a.Id==id);             //Method 2
        //    //Product? productFromDb2 = _db.Categories.Where(a=>a.Id==id).FirstOrDefault();     //Method 3
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost, ActionName("Delete")]     //Explicitly defined the End point 'Delete' Action
        //public IActionResult DeletePost(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index");
        //}

        //API Handles
        #region API CALLS       

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);   //based on ID we are getting the Product 
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });   //If Product to be Delete NULL, ERROR!
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageURL.TrimStart('\\'));
            //Delete the Old Image
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);           //Delete image record from DB
            }

            _unitOfWork.Product.Remove(productToBeDeleted);      // Pass the Product that should be Deleted!
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successfull" });

        }
        #endregion
    }
}
