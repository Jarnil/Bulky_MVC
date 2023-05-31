using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }

        // GET
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            /*Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
              Category? categoryFromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();*/

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _categoryRepo.Get(u => u.Id == id);

            if (obj == null)
                return NotFound();

            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
