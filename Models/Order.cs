using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime ThoiGianTao { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public List<DetailOrder> DetailOrders { get; set; }
    }
}
