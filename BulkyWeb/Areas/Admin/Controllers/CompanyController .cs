using Bulky.DataAccess.Data;
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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;           
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();   //which repository we are working on (Company Repo...)
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)     //Default [HttpGet]   //Create removed and Update + Insert added
        {          

            if (id == null || id == 0)
            {
                //Create
                return View(new Company());
            }
            else
            { 
                //Update
                Company companyObj = _unitOfWork.Company.Get(u=>u.Id==id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {              
                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }

                else 
                {
                    _unitOfWork.Company.Update(companyObj);
                }               
                _unitOfWork.Save();
                TempData["Success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }

            else 
            {
                return View(companyObj);
            }
            
        }       

        //API Handles
        #region API CALLS       

        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);   //based on ID we are getting the Company 
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });   //If Company to be Delete NULL, ERROR!
            }           
            _unitOfWork.Company.Remove(CompanyToBeDeleted);      // Pass the Company that should be Deleted!
            _unitOfWork.Save();

            return Json(new { success = true, Message = "Delete Successfull" });
        }
        #endregion
    }
}
