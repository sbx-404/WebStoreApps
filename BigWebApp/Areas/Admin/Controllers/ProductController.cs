using BigApp.DataAccess.Data;
using BigApp.DataAccess.Repository.IRepository;
using BigApp.Models;
using BigApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BigWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _IUnitOfWork;      // We add dependency injection it will change ICategoryRepository to CategoryRepository. 
        private readonly IWebHostEnvironment _webHostEnvironment; 
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _IUnitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objCategoryList = _IUnitOfWork.Product.GetAll().ToList();
            return View(objCategoryList);
        }

        public ActionResult Upsert(int? id)
        {
            
            ProductVM ProductVM = new()
            {
                CategoryLists = _IUnitOfWork.Category.GetAll().Select(u =>
                    new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                // create
                return View(ProductVM);
            }
            else
            {
                // update
                ProductVM.Product = _IUnitOfWork.Product.Get(u => u.Id == id);
                return View(ProductVM);
            }

        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(ProductVM obj, IFormFile? File)
        {
           if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;    // wwwroot path
                if(File != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);           // To give us random name for file. 
                    string productPath = Path.Combine(wwwRootPath, @"images\product");                      // combine two strings into product path.


                    // Open the file  and create it.
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create)) { 
                    File.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName;    // save image in your local and in model database.
                }
                _IUnitOfWork.Product.Add(obj.Product);
                _IUnitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {

                obj.CategoryLists = _IUnitOfWork.Category.GetAll().Select(u =>
                    new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });

                return View(obj); 
            }
        } 


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var DataFromDb = _IUnitOfWork.Product.Get(u => u.Id == id);
            if (DataFromDb == null)
            {
                return NotFound();
            }
            return View(DataFromDb);

        }
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Delete (StudentTable obj)
        // {
        //         _db.StudentTodos.Remove(obj);
        //         _db.SaveChanges();
        //         return RedirectToAction("Index");
        // }

        // this another way to delete data
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int? id)
        {
            var DataFromDb = _IUnitOfWork.Product.Get(u => u.Id == id);
            if (DataFromDb == null)
            {
                return NotFound();
            }
            _IUnitOfWork.Product.Remove(DataFromDb);
            _IUnitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}
