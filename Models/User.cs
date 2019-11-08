using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(50)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh!")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới tính!")]
        [MaxLength(5)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [StringLength(10, ErrorMessage = "Số điện thoại gồm 10 chữ số" ,MinimumLength = 10)]
        [RegularExpression(@"^[0]+[0-9]*$", ErrorMessage = "Số điện thoại bắt đầu bằng số 0 và chỉ chứa chữ số!")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email!")]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public override string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập tài khoản!")]
        //[Display(Name = "Tài khoản")]
        //[MaxLength(20)]
        //public string TaiKhoan { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Mật khẩu")]
        //public string MatKhau { get; set; }

        //[Display(Name = "Quyền hạn")]
        //public int RoleID { get; set; }

        //public Role Role { get; set; }

        public List<Order> Orders { get; set; }
    }
}
