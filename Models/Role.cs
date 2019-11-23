using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
