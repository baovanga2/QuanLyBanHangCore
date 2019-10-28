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
        public string Ten { get; set; }

        public string SDT { get; set; }

        public List<Order> Orders { get; set; }
    }
}
