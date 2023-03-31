using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace doan4.Areas.StatisticReports.Controllers
{
    [Area("StatisticReport")]
    [Route("admin/statistic-report/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class StatisticController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: StatisticController
        public IActionResult StatisticOrder()
        {
            return View();
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

        // POST: StatisticController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatisticController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatisticController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
