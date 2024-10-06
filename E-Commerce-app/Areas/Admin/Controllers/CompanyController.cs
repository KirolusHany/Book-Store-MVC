using E_Commerce_app.DataAccess.Repository.IRepository;
using E_Commerce_app.Models.ViewModels;
using E_Commerce_app.Models;
using E_Commerce_app.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_app.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var companytList = _unitOfWork.Company.GetAll().ToList();

            return View(companytList);
        }


        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update

                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company company, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                TempData["success"] = "company Created successfully";
                return RedirectToAction("Index");
            }
            else
            {
               
                return View(company);
            }

        }



        //public IActionResult DeleteImage(int imageId)
        //{
        //    var imageToBeDeleted = _unitOfWork.companyImage.Get(u => u.Id == imageId);
        //    int companyId = imageToBeDeleted.companyId;
        //    if (imageToBeDeleted != null)
        //    {
        //        if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
        //        {
        //            var oldImagePath =
        //                           Path.Combine(_webHostEnvironment.WebRootPath,
        //                           imageToBeDeleted.ImageUrl.TrimStart('\\'));

        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }
        //        }

        //        _unitOfWork.companyImage.Remove(imageToBeDeleted);
        //        _unitOfWork.Save();

        //        TempData["success"] = "Deleted successfully";
        //    }

        //    return RedirectToAction(nameof(Upsert), new { id = companyId });
        //}



        #region API CALLS
        public IActionResult GetAll()
        {
            List<Company> objcompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objcompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var company = _unitOfWork.Company.Get(i => i.Id == id);
            if (company == null)
            {
                return Json(new { successs = false, message = "Error while deleting" });
            }
           
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            return Json(new { successs = true, message = "company deleted successfully" });

        }

        #endregion
    }

}

