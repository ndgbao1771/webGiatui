using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using App.Data;
using App.AppContext;
using App.Models;

namespace CoffeeManager.Areas.Product.Controllers
{
    [Area("AdminCP")]
    [Route("config/shop/{action}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class ConfigShopController : Controller
    {
        private readonly AppDbContext _context;

        public ConfigShopController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult UnitProduct()
        {
            return View();
        }



        [HttpGet]
        public IActionResult GetUnitProductApi()
        {
            var unit = _context.unitIngredients.ToList();

            if (unit.Count <= 0 || unit == null)
            {
                return Json(new
                {
                    success = 0,
                    message = "không có đơn vị tính nào !"
                });
            }

            return Json(new
            {
                success = 1,
                units = unit
            });
        }

        [HttpPost]
        public IActionResult CreateUnitProductApi(string unit)
        {
            if (ModelState.IsValid)
            {
                if (unit != null && !_context.unitIngredients.Any(u => u.Unit == unit))
                {
                    _context.unitIngredients.Add(new UnitIngredient() { Unit = unit });
                    _context.SaveChanges();
                }

            }

            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteUnitProductApi(int id)
        {
            if (ModelState.IsValid)
            {
                var unit = _context.unitIngredients.Where(un => un.Id == id).FirstOrDefault();
                if (true)
                {
                    _context.Remove(unit);
                    _context.SaveChanges();
                }
            }

            return Ok();
        }

        public IActionResult SysInfoShop()
        {
            var getInfo = _context.infoShops.FirstOrDefault();
            return View(getInfo);
        }

        public IActionResult CreateInfo()
        {
            var getInfo = _context.infoShops.FirstOrDefault();
            if (getInfo == null) return View(new InfoShop { Id = 0 });
            return View(getInfo);
        }

        [HttpPost, ActionName(nameof(CreateInfo))]
        [ValidateAntiForgeryToken]
        public IActionResult CreateInfoConfirmed(int? id,[Bind("Id,ShopName,ShopPhoneNumber,ShopEmail,ShopAddress")] InfoShop infoShops)
        {
            if (ModelState.IsValid) {
                var getInfo = _context.infoShops.Count();
                if (getInfo < 1)
                {

                    _context.Add(infoShops);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(SysInfoShop));

                }

                if (id != null && id == infoShops.Id )
                {
                    var getInfoUp = _context.infoShops.Find(id);
                    if (getInfoUp == null)
                    {
                        return NotFound();
                    }
                    
                    getInfoUp.ShopName = infoShops.ShopName;
                    getInfoUp.ShopPhoneNumber = infoShops.ShopPhoneNumber;
                    getInfoUp.ShopEmail = infoShops.ShopEmail;
                    getInfoUp.ShopAddress = infoShops.ShopAddress;

                    _context.Update(getInfoUp);
                    _context.SaveChanges();
                    
                }
            }  
            return RedirectToAction(nameof(SysInfoShop));
        }


    }
}
