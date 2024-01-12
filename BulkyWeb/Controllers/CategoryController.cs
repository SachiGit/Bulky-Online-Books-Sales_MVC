using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();   //which repository we rae working on (Category Repo...)
            return View(objCategoryList);
        }
        
        public IActionResult Create()     //Default [HttpGet]
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayName.ToString()) 
            {
                ModelState.AddModelError("Name","The Display Order Cannot Exactly Match the Category Name!");
            }

            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test is an Invalid Value!");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);  //which repository we rae working on (Category Repo...)
                _unitOfWork.Save();
                TempData["Success"] = "Category Created Successfully";          
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u=>u.Id==id);                             //Method 1
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(a=>a.Id==id);             //Method 2
            //Category? categoryFromDb2 = _db.Categories.Where(a=>a.Id==id).FirstOrDefault();     //Method 3
            if (categoryFromDb == null)
            {  
                return NotFound(); 
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //if (obj.Name == obj.DisplayName.ToString())
            //{
            //    ModelState.AddModelError("Name", "The Display Order Cannot Exactly Match the Category Name!");
            //}

            //if (obj.Name.ToLower() == "test")
            //{
            //    ModelState.AddModelError("", "Test is an Invalid Value!");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Edited Successfully";
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);                         //Method 1
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(a=>a.Id==id);             //Method 2
            //Category? categoryFromDb2 = _db.Categories.Where(a=>a.Id==id).FirstOrDefault();     //Method 3
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]     //Explicitly defined the End point 'Delete' Action
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null) 
            {
             return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");           
        }
    }
}
