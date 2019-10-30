using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Required]
        [MaxLength(5)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Số điện thoại gồm 10 chữ số" ,MinimumLength = 10)]
        [RegularExpression(@"^{0}+[0-9""'\s-]*$")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Required]
        [Display(Name = "Tài khoản")]
        [MaxLength(20)]
        public string TaiKhoan { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        public int RoleID { get; set; }

        [Display(Name = "Quyền hạn")]
        public Role Role { get; set; }

        public List<Order> Orders { get; set; }
    }
}
