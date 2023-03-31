using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    [Table("ElectricalEquipment")]
    public class ElectricalEquipment
    {
        public enum StatusEquip
        {
            Active,
            NonActive
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên thiết bị")]
        public string EquipmentName { get; set; }

        [Required]
        [Display(Name = "Khối lượng giặt")]
        public float WashingVolume { get; set;}

        [Display(Name = "Trạng thái")]
        public StatusEquip Status { get; set; }

        [Display(Name = "Loại máy")]
        public int CategoryElecEquipID { get; set; }

        [Display(Name = "Loại máy")]
        [ForeignKey("CategoryElecEquipID")]
        public CategoryElectricEquipment cateElecEquipments { get; set; }

    }
}
