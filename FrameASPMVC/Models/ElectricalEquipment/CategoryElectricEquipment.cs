using App.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace App.Models
{
    [Table("CateElecEquip")]
    public class CategoryElectricEquipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Loại máy")]
        public string CateElecEquipName { get; set; }

        public List<ElectricalEquipment> ElecEquipOfCate { get; set; }
    }
}
