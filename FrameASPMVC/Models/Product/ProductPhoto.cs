using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Product
{
    [Table("ProductPhoto")]
    public class ProductPhoto{

        [Key]
        public int Id {get; set;}

        public string FileName {get; set;}

        public int ProductIds {get; set;}
        [ForeignKey("ProductIds")]
        public Products products {get; set;}
    }
}