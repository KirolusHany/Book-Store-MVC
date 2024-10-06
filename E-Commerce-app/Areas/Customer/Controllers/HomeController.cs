    using E_Commerce_app.DataAccess.Repository.IRepository;
    using E_Commerce_app.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Security.Claims;
using E_Commerce_app.Utility;


namespace E_Commerce_app.Areas.Customer.Controllers
    {
        [Area("Customer")]
    
        public class HomeController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
            {
                IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
                return View(productList);
            }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.AppUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.AppUserId == userId &&
           u.ProductId == shoppingCart.ProductId);
            if(cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();

            }
            else
            {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart,
            _unitOfWork.ShoppingCart.GetAll(u=>u.AppUserId==userId).Count());
            }
            TempData["success"] = "Cart updated successfully";

            return RedirectToAction("Index");
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
