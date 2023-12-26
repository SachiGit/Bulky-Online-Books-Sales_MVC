using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) 
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                TempData["Success"] = "Category Created Successfully";
                _db.SaveChanges();
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
            Category? categoryFromDb = _db.Categories.Find(id);                                   //Method 1
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
                _db.Categories.Update(obj);
                _db.SaveChanges();
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
            Category? categoryFromDb = _db.Categories.Find(id);                                   //Method 1
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
            Category? obj = _db.Categories.Find(id);
            if (obj == null) 
            {
             return NotFound();
            }
                _db.Categories.Remove(obj);
                _db.SaveChanges();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");           
        }
    }
}
