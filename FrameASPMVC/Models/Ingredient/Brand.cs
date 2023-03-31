using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace App.Models
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên thương hiệu")]
        public string BrandName { get; set; }

        public List<Material> IngredientOfBrand { get; set; }
    }
}
