using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class ProductWithCurrentPrice
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(100)]
        [RegularExpression(@"[\S].*", ErrorMessage = "Ký tự đầu tiên không thể là khoảng trắng!")]
        [Remote(action: "KiemTraTen", controller: "Products", AdditionalFields = nameof(ID))]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        public uint SoLuong { get; set; }

        [Display(Name = "Nhà sản xuất")]
        public int ProducerID { get; set; }

        public Producer Producer { get; set; }

        [Display(Name = "Loại")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá!")]
        [Display(Name = "Giá")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Chỉ chấp nhận số nguyên dương!")]
        public ulong Gia { get; set; }
    }
}