using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Product
{

    [Table("Product")]
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Tên sản phẩm")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }

        //chuỗi Url
        // [Required(ErrorMessage = "Phải tạo url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url hiển thị")]
        public string Slug { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; }

        //[Required]
        [Display(Name = "Người tạo sản phẩm")]
        public string AuthorId { set; get; }
        [ForeignKey("AuthorId")]
        [Display(Name = "Người tạo sản phẩm")]
        public AppUser Author { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdated { set; get; }

        [Display(Name = "Giá sản phẩm")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} có giá trị từ {1}")]
        [Column(TypeName = "decimal(10,0)")] // lưu trữ 18 chữ số trong cơ sở dữ liệu với 0 chữ số sau dấu thập phân
        public decimal Price {get; set;}

        public List<ProductCategory> ProductCategories {get; set;}

        public List<ProductPhoto> photo {get; set;}
    }
}