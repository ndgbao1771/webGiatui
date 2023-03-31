using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    [Table("UnitIngredient")]
    public class UnitIngredient
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Đơn vị")]
        public string Unit { get; set; }
       
    }
}
