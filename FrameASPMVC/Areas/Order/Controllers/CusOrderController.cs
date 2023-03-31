using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using App.Data;
using Microsoft.AspNetCore.Identity;
using App.AppContext;

namespace doan4.Areas.Orders.Controllers
{
    [Area("Order")]
    [Route("user/cusorder/[action]/{id?}")]
    public class CusOrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        

        public string StatusMessage {get; set;}

        public CusOrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders
                .Include(o => o.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        


        // GET: Order/Create
        [Authorize]
        public async  Task<IActionResult> SendOrder()
        {
            // ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            var user = await _userManager.GetUserAsync(this.User);
                if(user == null)
                {
                    return NotFound();
                }
            var infoOrder = new Order{
                Name = user.UserName,
                HomeAddress = user.HomeAdress,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateSend = DateTime.Now,
                DatePick = DateTime.Today.AddDays(1)
                
            };
            ViewBag.ServiceId = new SelectList(_context.washServices, "Id", "ServiceName");
            return View(infoOrder);
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOrder([Bind("Name,PhoneNumber,Email,DateSend,DatePick,Note,HomeAddress,StateOrder,VolumeOrderClothes,ServiceId")] Order order)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                if(user == null)
                {
                    return NotFound();
                }
                var services = await _context.washServices.FindAsync(order.ServiceId);
                if (services == null)
                {
                    return NotFound();
                }
                var Order = new Order()
                {
                    DateSend = DateTime.Now,
                    DatePick = order.DatePick,
                    AuthorId = user.Id,
                    Name = order.Name,
                    Note = order.Note,
                    HomeAddress = order.HomeAddress,
                    StateOrder = State.Process,
                    Email = order.Email,   
                    PhoneNumber = order.PhoneNumber,
                    VolumeOrderClothes = order.VolumeOrderClothes,
                    ServiceName = services.ServiceName,
                    ServicePrice = services.ServicePrice
                };
                
                _context.Add(Order);
                await _context.SaveChangesAsync();

                StatusMessage = "Bạn đã gửi đơn hàng";
                return RedirectToAction("Index", "Home");
            }
            // ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", order.AuthorId);
            return View(order);
        }


        [Authorize]
        public async Task<IActionResult> ViewOrder()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user != null)
            {
                var CusOrders = _context.orders
                            .Where(orders => orders.AuthorId == user.Id)
                            .ToList();
                return View(CusOrders);
            }
            return NotFound();
            
        }

        [HttpPost, ActionName("ViewOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewOrderConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(this.User);
            var cusOrder = await _context.orders.FirstOrDefaultAsync(oder => oder.Id == id);
            if(cusOrder != null)
            {
                if(cusOrder.AuthorId == user.Id)
                {
                    if(cusOrder.StateOrder == State.Process)
                    {
                        cusOrder.StateOrder = State.CCancel;
                    }
                }
            }
            _context.SaveChanges();

            var CusOrders = _context.orders
                            .Where(orders => orders.AuthorId == user.Id)
                            .ToList();
            return View(CusOrders);
        }



         private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.Id == id);
        }

    }

}
