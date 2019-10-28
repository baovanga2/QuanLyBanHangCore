using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Product
    {
        public int ID { get; set; }

        [Required]
        public string Ten { get; set; }

        [Required]   
        public ulong Gia { get; set; }

        [DefaultValue(0)]
        public int SoLuong { get; set; }
        public int ProducerID { get; set; }
        public Producer Producer { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public List<DetailOrder> DetailOrders { get; set; }
    }
}
