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
using App.Areas.Identity.Models.UserViewModels;
using App.AppContext;

namespace doan4.Areas.Orders.Controllers
{
    [Area("Order")]
    [Route("admin/order/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "page")]
        public int currentPage { get; set; }
        public int countPage { get; set; }  
        

        public string StatusMessage {get; set;}

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Order
        public async Task<IActionResult> Index([FromQuery(Name = "state")] string state, int pagesize, [FromQuery(Name = "page")] int curentPage, [FromQuery(Name = "infoSearch")] string infoSearch)
        {
            List<Order> orders = new List<Order>();
            if (state != null && state == "Process")
            {
                orders = _context.orders.Include(i=>i.issueAnInvoices).Where(o => o.StateOrder == State.Process)
                                              .OrderByDescending(o => o.DateSend)
                                              .ToList();
            }
            else if (state != null && state == "Confirm")
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).Where(o => o.StateOrder == State.Confirm)
                                              .OrderByDescending(o => o.DateSend)
                                              .ToList();
            }
            else if (state != null && state == "CCancel")
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).Where(o => o.StateOrder == State.CCancel)
                                              .OrderByDescending(o => o.DateSend)
                                              .ToList();
            }
            else if (state != null && state == "ACancel")
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).Where(o => o.StateOrder == State.ACancel)
                                              .OrderByDescending(o => o.DateSend)
                                              .ToList();
            }
            else if (state != null && state == "Finished")
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).Where(o => o.StateOrder == State.Finished)
                                              .OrderByDescending(o => o.DateSend)
                                              .ToList();
            }
            else if (state != null && state == "Paid")
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).Where(o => o.StateOrder == State.Paid)
                                              .OrderByDescending(o => o.DateSend)
                                              .ToList();
            }
            else if (state != null && state == "All")
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).OrderByDescending(o => o.DateSend).ToList();
            }
            else
            {
                orders = _context.orders.Include(i => i.issueAnInvoices).OrderByDescending(o => o.DateSend).ToList();
            }

            if (!string.IsNullOrEmpty(infoSearch))
            {
                orders = (from o in _context.orders.Include(i => i.issueAnInvoices).OrderByDescending(o => o.DateSend)
                          where o.Name.Contains(infoSearch) ||
                                 o.PhoneNumber.Contains(infoSearch) ||
                                 o.Email.Contains(infoSearch) ||
                                 o.HomeAddress.Contains(infoSearch)
                          select o)
                          
                          .ToList();

                int totalOrderS = orders.Count();
                if (pagesize <= 0) pagesize = 8;
                int countPagesS = (int)Math.Ceiling((double)totalOrderS / pagesize);

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
                ViewBag.totalorder = totalOrderS;

                var orderinPageS = orders.Skip((currentPage - 1) * pagesize)
                            .Take(pagesize)
                            .ToList();

                return View(orderinPageS);
            }
 
            int totalOrder = orders.Count();
            if (pagesize <= 0) pagesize = 8;
            int countPages = (int)Math.Ceiling((double)totalOrder / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;
            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    page = pageNumber,
                    pagesize = pagesize
                })
            };
            ViewBag.pagingModel = pagingModel;
            ViewBag.totalorder = totalOrder;

            var orderinPage = orders.Skip((currentPage - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();

            return View(orderinPage);
        }

        // GET: Order/Details/5
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
        public IActionResult Order(int? id) 
        {
            var getInfoOrder = _context.orders.FirstOrDefault(o => o.Id == id);
            var getOrder = new OrderDetails()
            {
                OrderId = getInfoOrder.Id,
                VolumeClothes = getInfoOrder.VolumeOrderClothes,
                PriceWash = getInfoOrder.ServicePrice * (decimal)getInfoOrder.VolumeOrderClothes
            };
            var getInfoEquipment = _context.electricalEquipments
                                           .Where(e => e.WashingVolume >= getInfoOrder.VolumeOrderClothes);
            ViewBag.EquipmentIds = new SelectList(getInfoEquipment, "Id", "EquipmentName");
            ViewBag.IngredientID = new SelectList(_context.materials, "Id", "Name");
            return View(getOrder);
        }
        
        [HttpPost, ActionName("Order")]
         public async Task<IActionResult> OrderConfirm(int? id, [Bind("Id,OrderId,VolumeClothes,EquipmentId,IngredientID,PriceWash")] OrderDetails orderDetails)
        {

            var user = await _userManager.GetUserAsync(this.User);
            var Order = await _context.orders.FirstOrDefaultAsync(oder => oder.Id == id);
            var getIngre =  _context.materials.FirstOrDefault(ingre => ingre.Id == orderDetails.IngredientID);
            if(id == null) return NotFound();
            if(Order != null)
            {
                Order.Supervisor = user.Id;
                Order.StateOrder = State.Confirm;
            }
            if(id != Order.Id)
            {
                return NotFound();
            };
            orderDetails.VolumeIngredient = Order.VolumeOrderClothes * getIngre.Amount;
            orderDetails.DateStartWash = DateTime.Now;
           
            getIngre.CurentWeight = (getIngre.CurentWeight * 1000 - orderDetails.VolumeIngredient)/1000;
            if(getIngre.CurentWeight < 0)
            {
                if(getIngre.Quantity > 0)
                {
                    getIngre.Quantity = getIngre.Quantity - 1;
                }
                else if(getIngre.Quantity == 0)
                {
                    _context.materials.Remove(getIngre);
                }
            }
            _context.materials.Update(getIngre);
            orderDetails.Id = 0;

			_context.orderDetails.Add(orderDetails);
            
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }


        // GET: Order/Create
        [HttpGet]
        [Authorize]
        public IActionResult CreateOrder()
        {
            ViewBag.ServiceId = new SelectList(_context.washServices, "Id", "ServiceName");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([Bind("Id,Name,PhoneNumber,Email,DateSend,DatePick,Note,HomeAddress,StateOrder,VolumeOrderClothes,ServiceId")] Order order)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return NotFound();
                }
                var services = await _context.washServices.FindAsync(order.ServiceId);
                if(services == null)
                {
                    return NotFound();
                }
                var Order = new Order()
                {
                    DateSend = order.DateSend,
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

                StatusMessage = "Tạo đơn hàng thành công";
                return RedirectToAction("Index", "Order");
            }
            // ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", order.AuthorId);
            return View(order);
        }


        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", order.AuthorId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateSend,HomeAddress,AuthorId,StateOrder")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", order.AuthorId);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var order = await _context.orders.FindAsync(id);
            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Finish")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishConfirmed(int? id)
        {
            var orderDetailFinish = await _context.orderDetails.Where(of => of.OrderId == id).FirstOrDefaultAsync();
            if(orderDetailFinish == null)
            {
                return NotFound();
            }
            orderDetailFinish.DateEndWash = DateTime.Now;

            var orderFinish = await _context.orders.Where(of => of.Id == id).FirstOrDefaultAsync();
            orderFinish.StateOrder = State.Finished;
            orderFinish.TotalPrice = orderDetailFinish.PriceWash;

            _context.orderDetails.Update(orderDetailFinish);
            _context.orders.Update(orderFinish);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Payment (int? id) 
        {
            var getOrder = await _context.orders.FindAsync(id);
            if (getOrder == null)
            {
                return NotFound();
            }
            var infoBill = new IssueAnInvoice()
            {
                CustomerName = getOrder.Name,
                CustomerAddress = getOrder.HomeAddress,
                CustomerEmail = getOrder.Email,
                CustomerPhone = getOrder.PhoneNumber,
                CustomerVolumeClothes = (int)getOrder.VolumeOrderClothes,
                TotalPriceWash = getOrder.TotalPrice,
                orderId = getOrder.Id
            };
            return View(infoBill);
        }

        [HttpPost, ActionName("Payment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentConfirmed(int? id, IssueAnInvoice issueAnInvoices)
        {
            if (id == null) return NotFound();
            if (ModelState.IsValid)
            {
                var changeStateOrder = await _context.orders.FindAsync(id);
                if (changeStateOrder == null) return NotFound();
                changeStateOrder.StateOrder = State.Paid;
                changeStateOrder.DateFinish = DateTime.Now;
                _context.Update(changeStateOrder);

                issueAnInvoices.Id = 0; // nếu ko sử dụng Bind thì phải trả về giá trị Id mặc định để Entity tự động cấp phát
                issueAnInvoices.DateCreateInvoice = DateTime.Now;
                
                _context.Add(issueAnInvoices);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportBill (int? id)
        {

            var getInfor = await _context.issueAnInvoices.Include(i=>i.orders)
                                                         .Where(i=>i.Id == id)
                                                         .FirstOrDefaultAsync();
            getInfor.DateCreateInvoice = DateTime.Now;
            return View(getInfor);
        }


        private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.Id == id);
        }


    }
}
