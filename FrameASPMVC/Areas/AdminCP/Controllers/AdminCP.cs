using App.AppContext;
using App.Data;
using App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace doan4.Areas.AdminCP.Controllers
{
    [Area("AdminCP")]
    [Route("admincp/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class AdminCP : Controller
    {
        private readonly AppDbContext _context;

        public AdminCP(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
        
            ViewData["getInfoStatictis"] = _context.issueAnInvoices.Sum(o => o.TotalPriceWash);

            ViewData["getInfoOrder"] = _context.orders.Where(o => o.StateOrder != State.Paid).Count();
            ViewData["getInfoOrderToday"] = _context.orders.Where(o => o.DateSend.Day == DateTime.Now.Day).Count();

            ViewData["getContact"] = _context.Contacts.Where(c => c.DateSent.Day == DateTime.Now.Day).Count();
            ViewData["getUser"] = _context.Users.Count();
            ViewData["getMoneyInMonth"] = _context.issueAnInvoices.Where(iss => iss.DateCreateInvoice.Month == DateTime.Now.Month).Sum(iss => iss.TotalPriceWash);
            return View();
        }

    }
}

   