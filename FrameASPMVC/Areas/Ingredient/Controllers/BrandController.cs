using App.AppContext;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FrameASPMVC.Areas.Ingredient.Controllers
{
    [Area("Ingredient")]
    [Route("admin/brand/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public string StatusMessage { get; set; }

        public BrandController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BrandController
        public IActionResult Index()
        {
            var brand = _context.brands.ToList();
            return View(brand);
        }

        // GET: BrandController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,BrandName")] Brand brand)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var brands = new Brand()
                    {
                        BrandName = brand.BrandName
                    };
                }
                _context.Add(brand);
                _context.SaveChanges();
                StatusMessage = "Tạo nhãn hàng thành công";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.brands == null)
            {
                return NotFound();
            }
            var brand = await _context.brands.FindAsync(id);
            return View(brand);
        }

        // POST: BrandController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, BrandName")] Brand brand)
        {
            if (id != brand.Id) return NotFound();
            if(ModelState.IsValid)
            {
                var brands = await _context.brands.FindAsync(id);
                if (brands == null) return NotFound();
                brands.BrandName = brand.BrandName;
                _context.Update(brands);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: BrandController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if(id == null) { return NotFound(); }
            var brand = await _context.brands.Where(b => b.Id == id).FirstOrDefaultAsync();
            return View(brand);
        }

        // POST: BrandController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConFirm(int? id)
        {
            var deleteBrand = await _context.brands.FindAsync(id);
            _context.brands.Remove(deleteBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
