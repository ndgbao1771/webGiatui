using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.ServiceStores
{
    [Table("ServiceStore")]
    public class ServiceStore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên dịch vụ")]
        public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Gía dịch vụ")]
        [Column(TypeName = "decimal(10,0)")]
        public decimal ServicePrice { get; set; }
    }
}
