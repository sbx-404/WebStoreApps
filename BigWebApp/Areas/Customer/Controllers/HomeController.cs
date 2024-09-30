using BigApp.DataAccess.Repository.IRepository;
using BigApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BigWebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _IUnitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _IUnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _IUnitOfWork.Product.GetAll("Category");
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            //Product product = _IUnitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
            Product product = _IUnitOfWork.Product.Get(u => u.Id == id, "Category");
            return View(product);
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
