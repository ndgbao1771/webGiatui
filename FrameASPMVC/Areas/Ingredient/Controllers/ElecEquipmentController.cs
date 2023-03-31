using App.AppContext;
using App.Data;
using App.Models;
using App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static App.Models.ElectricalEquipment;

namespace FrameASPMVC.Areas.Ingredient.Controllers
{
    [Area("Ingredient")]
    [Route("admin/electricequipment/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class ElecEquipmentController : Controller
    {
        private readonly AppDbContext _context;
        [BindProperty(SupportsGet = true, Name = "page")]
        public int currentPage { get; set; }
        public int countPage { get; set; }


        public string StatusMessage { get; set; }

        public ElecEquipmentController(AppDbContext context)
        {
            _context = context;
        }


        // GET: ElecEquipmentController
        public async Task<IActionResult> Index([FromQuery(Name = "state")] string state,[FromQuery(Name = "page")] int curentPage, int pagesize)
        {
            
            var appDbContext = _context.electricalEquipments.Include(e => e.cateElecEquipments)
                                                  .ToList();
            if (state != null && state == "NonActive")
            {
                appDbContext = _context.electricalEquipments.Where(o => o.Status == StatusEquip.NonActive)
                                              .OrderByDescending(o => o.WashingVolume)
                                              .ToList();
            }
            else if (state != null && state == "Active")
            {
                appDbContext = _context.electricalEquipments.Where(o => o.Status == StatusEquip.Active)
                                              .OrderByDescending(o => o.WashingVolume)
                                              .ToList();
            }
            else
            {
                appDbContext = _context.electricalEquipments.OrderByDescending(o => o.WashingVolume).ToList();
            }

            int totalElecEquip = appDbContext.Count();
            if (pagesize <= 0) pagesize = 8;
            int countPagesS = (int)Math.Ceiling((double)totalElecEquip / pagesize);

            if (currentPage > countPagesS) currentPage = countPagesS;
            if (currentPage < 1) currentPage = 1;
            var pagingModelS = new PagingModel()
            {
                countpages = countPagesS,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    page = pageNumber,
                    pagesize = pagesize
                })
            };
            ViewBag.pagingModel = pagingModelS;
            ViewBag.totalorder = totalElecEquip;

            var EquipmentInPageS = appDbContext.Skip((currentPage - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

            return View(EquipmentInPageS);
        }

        // GET: ElecEquipmentController/Create
        public IActionResult Create()
        {
            ViewBag.CategoryElecEquipID = new SelectList(_context.categoryElectricEquipments, "Id", "CateElecEquipName");
            return View();
        }

        // POST: Ingredients/Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm([Bind("Id,EquipmentName,WashingVolume,CategoryElecEquipID")] ElectricalEquipment equipment)
        {
            if (ModelState.IsValid)
            {
                equipment.Status = ElectricalEquipment.StatusEquip.NonActive;
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryElecEquipID"] = new SelectList(_context.categoryElectricEquipments, "Id", "CateElecEquipName", equipment.CategoryElecEquipID);
            return View(equipment);
        }

        // GET: ElecEquipmentController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || _context.electricalEquipments == null)
            {
                return NotFound();
            }

            var equipment = await _context.electricalEquipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            
            ViewData["CategoryElecEquipID"] = new SelectList(_context.categoryElectricEquipments, "Id", "CateElecEquipName", equipment.CategoryElecEquipID);
            return View(equipment);
        }

        // POST: ElecEquipmentController/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirm(int id, [Bind("Id,EquipmentName,WashingVolume,CategoryElecEquipID")] ElectricalEquipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryElecEquipID"] = new SelectList(_context.categoryElectricEquipments, "Id", "CateElecEquipName", equipment.CategoryElecEquipID);
            return View(equipment);
        }

        // GET: ElecEquipmentController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.electricalEquipments == null)
            {
                return NotFound();
            }

            var equipment = await _context.electricalEquipments
                .Include(e => e.cateElecEquipments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: ElecEquipmentController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.electricalEquipments == null)
            {
                return Problem("Entity set 'AppDbContext.electricalEquipments'  is null.");
            }
            var equipment = await _context.electricalEquipments.FindAsync(id);
            if (equipment != null)
            {
                _context.electricalEquipments.Remove(equipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
