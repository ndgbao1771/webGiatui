using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace App.ExtendMethods
{
   public static class PriceUnit {
        public static string ToPriceUnit (this decimal Price) {

            string price = string.Format("{0:0,0}", Price);
            return price + " đ";
        }
    }
}