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

        [Required]
        [MaxLength(50)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [StringLength(10, ErrorMessage = "Số điện thoại gồm 10 chữ số", MinimumLength = 10)]
        [RegularExpression(@"^[0]+[0-9""'\s-]*$")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        public List<Order> Orders { get; set; }
    }
}
