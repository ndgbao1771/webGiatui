using App.AppContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace doan4.Components.infoShopRate
{
    public class infoShopRate : ViewComponent
    {
        private readonly AppDbContext _context;

        public infoShopRate(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var getInforShopRate = _context.Contacts.OrderByDescending(r => r.DateSent).Take(6).ToList();
            return View(getInforShopRate);
        }
    }
}
