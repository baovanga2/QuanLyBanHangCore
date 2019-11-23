using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(50)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh!")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [StringLength(10, ErrorMessage = "Số điện thoại gồm 10 chữ số!", MinimumLength = 10)]
        [RegularExpression(@"[0][0-9]*", ErrorMessage = "Số điện thoại bắt đầu bằng số 0, chỉ chứa các chữ số!")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email!")]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tài khoản!")]
        [Display(Name = "Tài khoản")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Tài khoản chỉ chứa chỉ cái và chữ số")]
        [Remote(action: "KiemTraTaiKhoan", controller: "Users")]
        [MaxLength(20)]
        public string TaiKhoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu!")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không trùng nhau!")]
        public string XacNhanMatKhau { get; set; }
    }
}