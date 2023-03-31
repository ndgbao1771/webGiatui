using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("CategoryIngredient")]
    public class CategoryIngredient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Loại nguyên liệu")]
        public string CategoryName { get; set; }

        public List<Material> IngredientOfCate { get; set; }


    }
}
