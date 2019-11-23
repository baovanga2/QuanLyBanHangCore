using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class Role
    {
        public int ID { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        public List<User> Users { get; set; }
    }
}