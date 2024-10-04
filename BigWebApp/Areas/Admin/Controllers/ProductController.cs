using BigApp.DataAccess.Data;
using BigApp.DataAccess.Repository.IRepository;
using BigApp.Models;
using BigApp.Models.ViewModels;
using BigApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;


namespace BigWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            //IEnumerable<Product> objCategoryList = _IUnitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            //List<Product> objCategoryList = _IUnitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            //List<Product> objCategoryList = _IUnitOfWork.Product.GetAll("Category").ToList();
            IEnumerable<Product> objCategoryList = _IUnitOfWork.Product.GetAll("Category").ToList();
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
            if (id != null)
            {
                // update
                ProductVM.Product = _IUnitOfWork.Product.Get(u => u.Id == id);
                return View(ProductVM);
            }
            return View(ProductVM);
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

                if (!string.IsNullOrEmpty(obj.Product.ImageUrl)) {

                    //delete old image
                    var oldImgPath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImgPath))   //check if exist or not.
                    {
                        System.IO.File.Delete(oldImgPath);   // delete the file
                    }
                }

                    // Open the file  and create it.
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create)) { 
                    File.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName;    // save image in your local and in model database.
                }

                if (obj.Product.Id == 0)
                {
                _IUnitOfWork.Product.Add(obj.Product);

                }
                else
                {
                _IUnitOfWork.Product.Update(obj.Product);

                }

                _IUnitOfWork.Save();
                 TempData["success"] = "Product Created successfully";

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


        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var DataFromDb = _IUnitOfWork.Product.Get(u => u.Id == id);
        //    if (DataFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(DataFromDb);

        //}


        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Delete (StudentTable obj)
        // {
        //         _db.StudentTodos.Remove(obj);
        //         _db.SaveChanges();
        //         return RedirectToAction("Index");
        // }


        // this another way to delete data
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeletePost(int? id)
        //{
        //    var DataFromDb = _IUnitOfWork.Product.Get(u => u.Id == id);
        //    if (DataFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    _IUnitOfWork.Product.Remove(DataFromDb);
        //    _IUnitOfWork.Save();
        //    return RedirectToAction("Index");
        //}

        
        ////#region APICalls
        
        [HttpGet]
        public IActionResult Getall()
        {
            IEnumerable<Product> objCategoryList = _IUnitOfWork.Product.GetAll("Category").ToList();
            return Json(new { data = objCategoryList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _IUnitOfWork.Product.Get(u => u.Id == id);
            if(productToBeDeleted == null)  
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //delete old image
            var oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath , productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImgPath))   //check if exist or not.
            {
                System.IO.File.Delete(oldImgPath);   // delete the file
            }

            _IUnitOfWork.Product.Remove(productToBeDeleted);
            _IUnitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });

        }
            //Api End here
    }
}
