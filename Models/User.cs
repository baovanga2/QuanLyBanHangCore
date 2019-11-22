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
        [MaxLength(50)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày sinh")]
        public DateTime NgaySinh { get; set; }
        [MaxLength(5)]
        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }
        [StringLength(10, ErrorMessage = "Số điện thoại gồm 10 chữ số" ,MinimumLength = 10)]
        [RegularExpression(@"^[0]+[0-9]*$", ErrorMessage = "Số điện thoại bắt đầu bằng số 0 và chỉ chứa chữ số!")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        public List<Order> Orders { get; set; }
    }
}
