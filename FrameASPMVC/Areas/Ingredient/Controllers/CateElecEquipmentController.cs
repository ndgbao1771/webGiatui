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
    [Route("admin/CateE/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class CateElecEquipment : Controller
    {
        public readonly AppDbContext _context;

        public CateElecEquipment(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cates = await _context.categoryElectricEquipments.ToListAsync();
            return View(cates);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirm([Bind("CateElecEquipName")] CategoryElectricEquipment cateElecEquip)
        {
            var cate = new CategoryElectricEquipment()
            {
                CateElecEquipName = cateElecEquip.CateElecEquipName,
            };
            _context.Add(cate);
            _context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            var cateE = _context.categoryElectricEquipments.FirstOrDefault(c => c.Id == id);
            if (cateE == null)
            {
                return NotFound();
            }
            return View(cateE);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditConfirm(int? id, [Bind("Id","CateElecEquipName")] CategoryElectricEquipment cateElecEquip)
        {
            if(ModelState.IsValid)
            {
                var cateE = _context.categoryElectricEquipments.Find(id);
                if(cateE == null) return NotFound(); 
                cateE.CateElecEquipName = cateElecEquip.CateElecEquipName;
                _context.Update(cateE);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var cateE =  _context.categoryElectricEquipments.Where(c=>c.Id == id).FirstOrDefault();
            return View(cateE);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm(int? id)
        {
           if(id  == null)
            {
                return NotFound();
            }
            var cateE = _context.categoryElectricEquipments
                                .Where(c => c.Id == id)
                                .FirstOrDefault();
            _context.Remove(cateE);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
