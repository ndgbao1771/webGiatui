using App.AppContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace doan4.Component.infoShopFooter
{
    public class infoShopFooter : ViewComponent
    {
        private readonly AppDbContext _context;

        public infoShopFooter(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var getInforShop = _context.infoShops.FirstOrDefault();
            return View(getInforShop);
        }
    }
}
