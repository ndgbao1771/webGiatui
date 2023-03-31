using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("WashService")]
    public class WashService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên dịch vụ")]
        public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Giá dịch vụ")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal ServicePrice { get; set; }

        [Required]
        [Display(Name = "Mô tả dịch vụ")]
        public string ServiceDescription { get; set; }

    }
}
