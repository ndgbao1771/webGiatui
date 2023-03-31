using App.AppContext;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FrameASPMVC.Areas.Ingredient.Controllers
{
    [Area("Ingredient")]
    [Route("admin/Cate/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class CateIngredient : Controller
    {
        public readonly AppDbContext _context;

        public CateIngredient(AppDbContext context)
        {
            _context = context;
        }

        // GET: CategoryIngredientController
        public async Task<IActionResult> Index()
        {
            var cates = await _context.cateIngredients.ToListAsync();
            return View(cates);
        }

        // GET: CategoryIngredientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryIngredientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryIngredientController/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirm([Bind("CategoryName")] CategoryIngredient cateIngre)
        {
            var cate = new CategoryIngredient()
            {
                CategoryName = cateIngre.CategoryName,
            };
            _context.Add(cate);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryIngredientController/Edit/5
        public IActionResult Edit(int? id)
        {
            var cate = _context.cateIngredients.FirstOrDefault(c => c.Id == id);
            if (cate == null)
            {
                return NotFound();
            }
            return View(cate);
        }

        // POST: CategoryIngredientController/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfirm(int? id, [Bind("Id, CategoryName")] CategoryIngredient cateIngre)
        {
            if(ModelState.IsValid)
            {
                var cate = _context.cateIngredients.Find(id);
                if(cate == null) return NotFound(); 
                cate.CategoryName  = cateIngre.CategoryName;
                _context.Update(cate);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryIngredientController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var cate =  _context.cateIngredients.Where(c=>c.Id == id).FirstOrDefault();
            return View(cate);
        }

        // POST: CategoryIngredientController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int? id)
        {
           if(id  == null)
            {
                return NotFound();
            }
            var cate = _context.cateIngredients
                                .Where(c => c.Id == id)
                                .FirstOrDefault();
            _context.Remove(cate);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
