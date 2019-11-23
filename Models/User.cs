using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class User : IdentityUser<int>
    {
        [Display(Name = "Tên")]
        public string Ten { get; set; }

        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public List<Order> Orders { get; set; }
    }
}