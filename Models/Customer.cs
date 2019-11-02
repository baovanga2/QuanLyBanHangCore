using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [MaxLength(50)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [StringLength(10, ErrorMessage = "Số điện thoại gồm 10 chữ số!", MinimumLength = 10)]
        [RegularExpression(@"^[0]+[0-9]*$", ErrorMessage = "Số điện thoại bắt đầu bằng số 0, chỉ chứa các chữ số!")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        public List<Order> Orders { get; set; }
    }
}
