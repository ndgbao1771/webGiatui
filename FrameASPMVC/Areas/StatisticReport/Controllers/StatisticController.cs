using App.AppContext;
using App.Data;
using App.Models;
using App.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace doan4.Areas.StatisticReports.Controllers
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

        public StatisticController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult StatisticOrder(int pagesize, [FromQuery(Name = "page")] int curentPage, [FromQuery(Name = "filter")] string filter)
        {
            List<Order> orders = new List<Order>();
            if (filter != null && filter == "StatisticDay")
            {
                orders = _context.orders.Where(d => d.DateFinish.Day == DateTime.Now.Day && d.StateOrder == State.Paid)
                                        .OrderByDescending(d => d.DateFinish)
                                        .ToList();
            }else if(filter != null && filter == "StatisticMonth")
            {
                orders = _context.orders.Where(d => d.DateFinish.Month == DateTime.Now.Month && d.StateOrder == State.Paid)
                                        .OrderByDescending(d => d.DateFinish)
                                        .ToList();
            }
            else if (filter != null && filter == "StatisticYear")
            {
                orders = _context.orders.Where(d => d.DateFinish.Year == DateTime.Now.Year && d.StateOrder == State.Paid)
                                        .OrderByDescending(d => d.DateFinish)
                                        .ToList();
            }
            else
            {
                orders = _context.orders.Where(d => d.StateOrder == State.Paid)
                                        .OrderByDescending(d => d.DateFinish)
                                        .ToList();
            }
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

        // GET: StatisticController/Details/5
        public IActionResult StatisticIngredient()
        {
            return View();
        }

        // GET: StatisticController/Create
        public IActionResult StatisticFinance()
        {
            return View();
        }

        public IActionResult ExportReport()
        {
            return View();
        }

        
    }
}
