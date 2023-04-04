using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Orders {

    public enum State {
        Process,
        Confirm,
        Finished,
        Paid,
        CCancel,
        ACancel
    }
    [Table("Order")]
    public class Order 
    {   
        [Key]
        public int Id {get; set;}

        [Display(Name = "Họ và tên", Prompt = "Nhập họ và tên của bạn ...")]
        public string Name {get; set;}

        [Display(Name = "Khối lượng đồ gửi")]
        [Required(ErrorMessage = "Phải ghi rõ khối lượng đồ muốn giặt")]
        public float VolumeOrderClothes { get; set;}

        [Display(Name = "Ngày gửi đồ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateSend {get; set;}

        [Display(Name = "Mong muốn lấy đồ ngày")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DatePick {get; set;}

        [Display(Name = "Ngày hoàn thành")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateFinish { get; set; }

        [Required(ErrorMessage = "Phải điền thông tin {0}")]
        [Display(Name = "Địa chỉ", Prompt = "Ghi rõ thông tin địa chỉ tại đây...")]
        public string HomeAddress {get; set;}

        [Display(Name = "Lưu ý của bạn", Prompt = "Ví dụ: Hãy giặt đồ của tôi bằng nước ấm...")]
        public string Note {get; set;}

        [Display(Name = "Người tạo đơn hàng")]
        public string AuthorId { set; get; }
        [ForeignKey("AuthorId")]
        [Display(Name = "Người tạo đơn hàng")]
        public AppUser Author { set; get; }

        [Display(Name = "Người thực hiện")]
        public string Supervisor { get; set; }

        [Display(Name = "Trạng thái đơn hàng")]
        public State StateOrder {get; set;}

        [Display(Name = "Email", Prompt = "Nhập email ...")]
        public string Email {get; set;}

        [Display(Name = "Số điện thoại", Prompt = "Nhập số điện thoại ...")]
        public string PhoneNumber {get; set;}

        [Display(Name = "Tên dịch vụ")]
        public string ServiceName { get; set;}

        [Display(Name = "Gía dịch vụ")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal ServicePrice { get; set; }

        [Display(Name = "Tổng tiền")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal TotalPrice { get; set;}


        [NotMapped]
        [Display(Name = "Loại dịch vụ")]
        public int ServiceId { get; set; }

        public List<IssueAnInvoice> issueAnInvoices { get; set; }

    }
}
