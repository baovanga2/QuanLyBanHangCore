using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [Range(1, 1000, ErrorMessage = "Chỉ chấp nhận số lượng để thêm từ 1 đến 1000!")]
        [Display(Name = "Số lượng cần thêm")]
        public uint SoLuongThem { get; set; }
    }
}
