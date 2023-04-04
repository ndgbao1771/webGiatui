using App.AppContext;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace doan4.Components.infoShopCount
{
    public class infoShopCount : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public infoShopCount(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public IViewComponentResult Invoke()
        {
            ViewData["accountCount"] = _context.Users.Count();
            ViewData["orderCount"] = _context.orders.Count();
            ViewData["contactCount"] = _context.Contacts.Count();
            return View();
        }
    }
}
