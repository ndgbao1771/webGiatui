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
            ViewData["getInfoOrder"] = _context.orders.Where(o => o.StateOrder == State.Paid && o.DateSend.Day == DateTime.Now.Day).Count();
            ViewData["getInfoOrder2"] = _context.orders.Where(o => o.DateSend.Day == DateTime.Now.Day).Count();

            var b = _context.issueAnInvoices.Where(o => o.DateCreateInvoice.Day == DateTime.Now.Day && o.DateCreateInvoice.Month == DateTime.Now.Month && o.DateCreateInvoice.Year == DateTime.Now.Year).Count();

            ViewData["getProgress"] = (float)_context.orders
                                              .Where(o => o.StateOrder == State.Paid).Count()
                                               / (float)_context.orders.Count() * 100;
            ViewData["getContact"] = _context.Contacts.Where(c => c.DateSent.Day == DateTime.Now.Day).Count();
            return View();
        }

    }
}

   