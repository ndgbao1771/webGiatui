using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
	[Table("InfoShop")]
	public class InfoShop
	{
		[Key]
		public int Id { get; set; }

		[Display(Name = "Tên chi nhánh")]
		[Required]
		public string ShopName { get; set; }

		[Required]
		[Display(Name = "Số điện thoại")]
		public string ShopPhoneNumber { get; set; }

		[Required]
		[Display(Name = "Địa chỉ")]
		public string ShopAddress { get; set; }
	}
}
