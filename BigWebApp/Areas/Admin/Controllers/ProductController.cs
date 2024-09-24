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

        public ProductController(IUnitOfWork unitOfWork)
        {
            _IUnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objCategoryList = _IUnitOfWork.Product.GetAll().ToList();
            return View(objCategoryList);
        }

        public ActionResult Create()
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
            return View(ProductVM);
        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM obj)
        {
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Product.Add(obj.Product);
                _IUnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Product.Update(obj);
                _IUnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
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
