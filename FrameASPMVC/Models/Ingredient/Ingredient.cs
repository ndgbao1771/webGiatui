using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("Ingredient")]
    public class Material
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên nguyên liệu")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Khối lượng mỗi sản phẩm")]
        public int Weight { get; set; }

        [Required]
        [Display(Name = "Tổng Khối lượng")]
        public float TotalWeight { get; set; }

        [Display(Name = "Tổng khối lượng hiện tại")]
        public float CurentWeight { get; set; }

        [Required]
        [Display(Name = "Khối lượng sử dụng / 1kg đồ")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "Số lượng nguyên liệu")]
        public int Quantity { get; set; }

        [Display(Name = "Ngày nhập")]
        public DateTime ImportDate { get; set; }

        [Required]
        [Display(Name = "Giá")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal IngredientPrice { get; set; }

        [Display(Name = "Tên thương hiệu")]
        public int brandID { get; set; }

        [Display(Name = "Tên thương hiệu")]
        [ForeignKey("brandID")]
        public Brand brands { get; set; }

        [Display(Name = "Loại nguyên liệu")]
        public int CategoryIngredientID { get; set; }

        [Display(Name = "Loại nguyên liệu")]
        [ForeignKey("CategoryIngredientID")]
        public CategoryIngredient cateIngredients { get; set; }

        [Display(Name = "Đơn vị tính của sản phẩm")]
        public int UnitIngredientWeightID { get; set; }
        public UnitIngredient unitIngredientWeight { get; set; }

        [Display(Name = "Đơn vị tính sử dụng")]
        public int UnitIngredientAmountID { get; set; }
        public UnitIngredient unitIngredientAmount { get; set; }

    }
}
