using App.AppContext;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace doan4.Areas.WashServices.Controllers
{
    [Area("WashService")]
    [Route("admin/washservice/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class WashServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _usermanager;

        public WashServiceController(AppDbContext context, UserManager<AppUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: WashServiceController
        public async Task<IActionResult> Index()
        {
            var services = await _context.washServices.ToListAsync();
            return View(services);
        }

        // GET: WashServiceController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var getInfoSer = await _context.washServices
                                           .Where(s => s.Id == id)
                                           .FirstOrDefaultAsync();
            if (getInfoSer == null) 
            {
                return NotFound();
            }
            return View(getInfoSer);
        }

        // GET: WashServiceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WashServiceController/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed([Bind("ServiceName, ServicePrice, ServiceDescription")] WashService washService)
        {
            if (ModelState.IsValid)
            {
                var services = new WashService()
                {
                    ServiceName = washService.ServiceName,
                    ServicePrice = washService.ServicePrice,
                    ServiceDescription = washService.ServiceDescription,
                };
                _context.washServices.Add(services);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)) ;
        }

        // GET: WashServiceController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null || _context.washServices == null)
            {
                return NotFound();
            }
            var infoService = await _context.washServices.FindAsync(id);
            if (infoService == null) 
            {
                return NotFound();
            }
            return View(infoService);
        }

        // POST: WashServiceController/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(int? id, [Bind("Id, ServiceName, ServicePrice, ServiceDescription")] WashService washService)
        {
            if(id != washService.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _context.Update(washService);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: WashServiceController/Delete/5
        public ActionResult Delete(int? id)
        {
            var services = _context.washServices.Where(s => s.Id == id).FirstOrDefault();
            if(services == null)
            {
                return NotFound();
            }
            return View(services);
        }

        // POST: WashServiceController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var services = await _context.washServices.FindAsync(id);
            _context.washServices.Remove(services);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
