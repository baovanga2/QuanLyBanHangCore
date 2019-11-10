using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Vui lòng nhập tài khoản!")]
        [Display(Name = "Tài khoản")]
        [MaxLength(20)]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Display(Name = "Ghi nhớ tôi")]
        public bool GhiNhoToi { get; set; }
    }
}
