using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.ViewModels
{
    public class PersonalDetailsViewModel
    {
        public PersonalDetailsViewModel()
        {
            Roles = new List<string>();
        }

        [Display(Name = "Tài khoản")]
        public string TaiKhoan { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
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