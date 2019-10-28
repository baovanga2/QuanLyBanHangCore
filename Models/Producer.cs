using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Producer
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        public List<Product> Products { get; set; }
    }
}
