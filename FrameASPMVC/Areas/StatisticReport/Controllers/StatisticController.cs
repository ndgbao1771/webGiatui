using App.AppContext;
using App.Data;
using App.Models;
using App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace doan4.StatisticReports.Controllers
{
    [Area("StatisticReport")]
    [Route("admin/statistic-report/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class StatisticController : Controller
    {
        private readonly AppDbContext _context;

        public const int ITEM_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "page")]
        public int currentPage { get; set; }
        public int countPage { get; set; }
        public object JsonRequestBehavior { get; private set; }

        public StatisticController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult StatisticOrder(int pagesize, [FromQuery(Name = "page")] int curentPage, [FromQuery(Name = "filter")] string filter)
        {
            List<Order> orders = new List<Order>();
            
            int totalOrder = orders.Count();
            if (pagesize <= 0) pagesize = 10;
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


        public IActionResult AjaxProcessCall(DateTime startDate, DateTime endDate)
        {

            if(startDate != endDate)
            {
                var orders = _context.orders.Where(o => o.DateFinish >= startDate && o.DateFinish <= endDate).Select(o => new infoOrder
                {
                    name = o.Name,
                    phoneNumber = o.PhoneNumber,
                    serviceName = o.ServiceName,
                    totalWash = o.VolumeOrderClothes,
                    totalPrice = o.TotalPrice,
                    datefinish = o.DateFinish,
                    state = o.StateOrder
                }).ToList();

                List<string> listlabelday = new List<string>();

                List<int> ordertotalpricechart = new List<int>();

                if (startDate < endDate)
                {
                    DateTime date1 = startDate;
                    while (date1 <= endDate)
                    {
                        
                        listlabelday.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + "/" + date1.Year.ToString());
                        ordertotalpricechart.Add( (int)orders.Where(o => o.datefinish.Day == date1.Day 
                                                                      && o.datefinish.Month == date1.Month 
                                                                      && o.datefinish.Year == date1.Year)
                                                             .Select(o => o.totalPrice).Sum());
                        date1 = date1.AddDays(1);

                    }
                }
                
                
                if (orders != null)
                {
                    return Json(new { error = 0,
                                      orderlist = orders,
                                      listdaylabel = listlabelday,
                                      ordertotalpricechart = ordertotalpricechart.ToArray(),
                                      countorder = _context.orders.Where(o => o.DateSend >= startDate && o.DateSend <= endDate).Count(),
                                      ordercancel = _context.orders.Where(o => o.DateSend >= startDate && o.DateSend <= endDate && o.StateOrder == State.CCancel ||
                                                                               o.DateSend >= startDate && o.DateSend <= endDate && o.StateOrder == State.ACancel)
                                                                   .Count(),
                                      ordertotalprice = (from ordertotalprice in orders
                                                         where ordertotalprice.state == State.Paid
                                                         select ordertotalprice.totalPrice).Sum()
                                      
                    });

                }
            }
            else if(startDate == endDate) 
            {
                var orders = _context.orders.Where(o => o.DateFinish.Day == startDate.Day && o.DateFinish.Month == startDate.Month && o.DateFinish.Year == startDate.Year).Select(o => new infoOrder
                {
                    name = o.Name,
                    phoneNumber = o.PhoneNumber,
                    serviceName = o.ServiceName,
                    totalWash = o.VolumeOrderClothes,
                    totalPrice = o.TotalPrice,
                    state = o.StateOrder
                }).ToList();

                List<string> listlabelday = new List<string>();

                List<int> ordertotalpricechart = new List<int>();

                DateTime date1 = startDate;
                while (date1 <= endDate)
                {

                    listlabelday.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + "/" + date1.Year.ToString());
                    ordertotalpricechart.Add((int)orders.Where(o => o.datefinish.Day == date1.Day
                                                                  && o.datefinish.Month == date1.Month
                                                                  && o.datefinish.Year == date1.Year)
                                                         .Select(o => o.totalPrice).Sum());
                    date1 = date1.AddDays(1);

                }

                if (orders != null)
                {

                    return Json(new { error = 0, orderlist = orders,
                                      listdaylabel = listlabelday,
                                      ordertotalpricechart = ordertotalpricechart.ToArray(),
                                      countorder = orders.Count(), 
                                      ordercancel = (from ordercancel in orders 
                                                    where ordercancel.state == State.CCancel || ordercancel.state == State.ACancel
                                                    select ordercancel).Count(),
                                      ordertotalprice = (from ordertotalprice in orders
                                                         where ordertotalprice.state == State.Paid
                                                         select ordertotalprice.totalPrice).Sum()
                    });

                }
            }
             
            return Json(new { error = 1});
        }

        public IActionResult StatisticIngredient(DateTime startDate, DateTime endDate)
        {
            if (startDate != endDate)
            {
                var orderdetail = _context.orderDetails;
                var materialUse = _context.materials;
                var ingredient = _context.materials.Select(i => new infoIngre
                {
                    nameIngre = i.Name,
                    weigh = i.Weight,
                    currentWeigh = i.CurentWeight,
                    totalWeigh = i.TotalWeight,
                    weighUsed = (i.TotalWeight*1000 - i.CurentWeight*1000)/1000
                }).ToList();

                List<string> listlabelday = new List<string>();

                List<int> listsumingre = new List<int>();

                if (startDate < endDate)
                {
                    DateTime date1 = startDate;
                    while (date1 <= endDate)
                    {

                        listlabelday.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + "/" + date1.Year.ToString());
                        listsumingre.Add((int)orderdetail.Where(o => o.DateStartWash.Day == date1.Day
                                                                      && o.DateStartWash.Month == date1.Month
                                                                      && o.DateStartWash.Year == date1.Year)
                                                             .Select(o => o.VolumeIngredient).Sum());
                        date1 = date1.AddDays(1);

                    }
                }

                return Json(new
                {
                    error = 0,
                    ingredient1 = ingredient,
                    listlabel = listlabelday,
                    ingreuse = listsumingre.ToArray(),
                    ingreuse2 = materialUse.Select(o => o.TotalWeight).Sum(),
                    materialuse = materialUse.Select(i => i.CurentWeight).Sum(),
                    ingreorderuse = orderdetail.Select(i => i.VolumeIngredient).Sum()
                });
            }
            else if (startDate == endDate)
            {
                var ingredient = _context.materials.Select(i => new infoIngre
                {
                    nameIngre = i.Name,
                    weigh = i.Weight,
                    currentWeigh = i.CurentWeight,
                    totalWeigh = i.TotalWeight,
                    weighUsed = (i.TotalWeight * 1000 - i.CurentWeight * 1000) / 1000
                }).ToList();
                var orderdetail = _context.orderDetails;
                var materialUse = _context.materials;


                var total = orderdetail.Where(o => o.DateStartWash.Day == startDate.Day 
                                                && o.DateStartWash.Month == startDate.Month 
                                                && o.DateStartWash.Year == startDate.Year)
                                       .Select(o => o.VolumeIngredient)
                                       .Sum();
                float ingredientUse = total / 1000;

                List<string> listlabelday = new List<string>();

                List<int> listsumingre = new List<int>();
                DateTime date1 = startDate;

                listlabelday.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + "/" + date1.Year.ToString());
                listsumingre.Add((int)orderdetail.Where(o => o.DateStartWash.Day == date1.Day
                                                              && o.DateStartWash.Month == date1.Month
                                                              && o.DateStartWash.Year == date1.Year)
                                                     .Select(o => o.VolumeIngredient).Sum());
                date1 = date1.AddDays(1);

                return Json(new
                {
                    error = 0,
                    listlabel = listlabelday,
                    ingredient1 = ingredient,
                    ingreuse = listsumingre.ToArray(),
                    ingreuse2 = materialUse.Select(o => o.TotalWeight).Sum(),
                    materialuse = materialUse.Select(i => i.CurentWeight).Sum(),
                    ingreorderuse = orderdetail.Where(i => i.DateStartWash.Day == startDate.Day 
                                                        && i.DateStartWash.Month == startDate.Month 
                                                        && i.DateStartWash.Year == startDate.Year)
                                               .Select(i => i.VolumeIngredient).Sum()
                });
            }
            
            return Json(new { error = 1 });
        }
   
    }

    class infoOrder
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
        public string serviceName { get; set; }
        public float totalWash { get; set; }
        public Decimal totalPrice { get; set; }
        public State state { get; set; }
        public DateTime datefinish { get; set; }
        
    }

    class infoIngre
    {
        
        public int id { get; set; }
        public string nameIngre { get; set; }
        public int weigh { get; set; }
        public float totalWeigh { get; set; }
        public float currentWeigh { get; set; }
        public float weighUsed { get; set; }

    }

}
