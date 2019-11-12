using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel()
        {
            Roles = new List<string>();
        }

        [Display(Name = "Tài khoản")]
        public string TaiKhoan { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Quyền hạn")]
        public IList<string> Roles { get; set; }
    }
}