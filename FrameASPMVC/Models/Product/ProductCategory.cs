using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Product
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        public int ProductId { set; get; }

        public int CategoryId { set; get; }

        [ForeignKey("ProductId")]
        public Products Product { set; get; }

        [ForeignKey("CategoryId")]
        public Category Category { set; get; }
    }
}