using App.Models.Orders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("BillPayment")]    
    public class IssueAnInvoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Display(Name = "Email khách hàng")]
        public string CustomerEmail { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Required]
        [Display(Name = "Khối lượng đồ")]
        public int CustomerVolumeClothes { get; set; }

        [Required]
        [Display(Name = "Tổng tiền thanh toán")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal TotalPriceWash { get; set; }

        [Display(Name = "Ngày lập")]
        public DateTime DateCreateInvoice { get; set; }

        [Display(Name = "Tiền khách đưa")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal MoneyGiveCustomer { get ; set; }
        
        [Display(Name = "Tiền trả lại")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal MoneyReturn { get; set; }

        [Display(Name = "Xuất hóa đơn")]
        public bool StateInvoice { get; set; }
		
		public int orderId { get; set; }
        [ForeignKey(nameof(orderId))]
        public Order orders { get; set; }


    }
}
