using App.AppContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace doan4.Components.infoShopContact
{
    public class infoShopContact : ViewComponent
    {
        private readonly AppDbContext _context;

        public infoShopContact(AppDbContext context)
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
