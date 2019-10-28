using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class DetailOrder
    {
        public int ID { get; set; }
        public ulong Gia { get; set; }
        public int SoLuong { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
