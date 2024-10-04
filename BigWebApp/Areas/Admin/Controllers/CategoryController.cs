using BigApp.DataAccess.Data;
using BigApp.DataAccess.Repository.IRepository;
using BigApp.Models;
using BigApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BigWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _IUnitOfWork;      // We add dependency injection it will change ICategoryRepository to CategoryRepository. 

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _IUnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _IUnitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public ActionResult Create()
        {
            
            return View();
        }

        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Category.Add(obj);
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
            var DataFromDb = _IUnitOfWork.Category.Get(u => u.Id == id);
            if (DataFromDb == null)
            {
                return NotFound();
            }
            return View(DataFromDb);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _IUnitOfWork.Category.Update(obj);
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
            var DataFromDb = _IUnitOfWork.Category.Get(u => u.Id == id);
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
            var DataFromDb = _IUnitOfWork.Category.Get(u => u.Id == id);
            if (DataFromDb == null)
            {
                return NotFound();
            }
            _IUnitOfWork.Category.Remove(DataFromDb);
            _IUnitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}
