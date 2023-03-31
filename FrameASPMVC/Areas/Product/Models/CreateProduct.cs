using System.ComponentModel.DataAnnotations;
using App.Models.Product;

namespace   App.Models.Product{
    public class CreateProduct : Products {
        
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs {get; set;}
    }
}