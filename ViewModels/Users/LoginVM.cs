using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.ViewModels
{
    public class LoginVM
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

        public string ReturnUrl { get; set; }
    }
}