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
        public string Ten { get; set; }

        [Required]
        [MaxLength(5)]
        public string GioiTinh { get; set; }

        [Required]
        public string SDT { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string DiaChi { get; set; }

        [Required]
        public string TaiKhoan { get; set; }

        [Required]
        public string MatKhau { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        public List<Order> Orders { get; set; }
    }
}
