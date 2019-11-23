using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class ProductAddQuantityViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Ten { get; set; }

        [Display(Name = "Số lượng hiện có")]
        public uint SoLuongCo { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng!")]
        [Range(1, 1000, ErrorMessage = "Chỉ chấp nhận số lượng tối đa là 1000!")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "Chỉ chấp nhận số nguyên dương!")]
        [Display(Name = "Số lượng cần thêm")]
        public uint SoLuongThem { get; set; }
    }
}