using App.Models.Orders;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace App.Models
{
    [Table("OrderDetail")]
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public int OrderId { set; get; }
        [ForeignKey("OrderId")]
        public Order orderIds {  get; set; }

        [Display(Name = "Mã máy")]
        public int EquipmentId { get; set; }
        [ForeignKey("EquipmentId")]
        public ElectricalEquipment equipments { get; set; }

        [Display(Name = "Mã nguyên liệu")]
        public int IngredientID { get; set; }
        [ForeignKey(nameof(IngredientID))]
        public Material ingredients { get; set; }

        [Display(Name = "Khối lượng sử dụng")]
        [Required]
        public float VolumeIngredient { get; set; }

        [Display(Name = "Khối lượng đồ được giặt")]
        [Required]
        public float VolumeClothes { get; set; }

        [Display(Name = "Bắt đầu giặt")]
        public DateTime DateStartWash { get; set; }

        [Display(Name = "Hoàn thành giặt")]
        public DateTime DateEndWash { get; set;}

        [Display(Name = "Giá giặt")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal PriceWash { get; set; }


    }
}
