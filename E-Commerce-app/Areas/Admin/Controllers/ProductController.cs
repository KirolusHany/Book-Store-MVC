using E_Commerce_app.DataAccess.Repository.IRepository;
using E_Commerce_app.Models;
using E_Commerce_app.Models.ViewModels;
using E_Commerce_app.Utility;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;


namespace E_Commerce_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(productList);
        }


        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
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
                //create
                return View(productVM);
            }
            else
            {
                //update

                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);    
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImgUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);    
                    }
                    productVM.Product.ImgUrl = @"\images\product\" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                         _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);  
                }
                _unitOfWork.Save();
                TempData["success"] = "product Created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll()
                    .Select(u => new SelectListItem 
                    { 
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
            return View(productVM);
            }

        }



        //public IActionResult DeleteImage(int imageId)
        //{
        //    var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
        //    int productId = imageToBeDeleted.ProductId;
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

        //        _unitOfWork.ProductImage.Remove(imageToBeDeleted);
        //        _unitOfWork.Save();

        //        TempData["success"] = "Deleted successfully";
        //    }

        //    return RedirectToAction(nameof(Upsert), new { id = productId });
        //}



        #region API CALLS
       [HttpGet]
        [Route("Admin/GetAll")]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.Product.Get(i=>i.Id==id);
            if (product == null)
            {
                return Json(new {successs= false , message="Error while deleting"});
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImgUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return Json(new { successs = true, message = "Product deleted successfully" });

        }

        #endregion
    }

}
