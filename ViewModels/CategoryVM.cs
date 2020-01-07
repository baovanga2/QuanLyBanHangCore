using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.ViewModels
{
    public class CategoryVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(20)]
        [Remote(action: "KiemTraTen", controller: "Categories", AdditionalFields = nameof(ID))]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
    }
}