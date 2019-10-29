using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class Order
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Thời gian")]
        public DateTime ThoiGianTao { get; set; }

        public int UserID { get; set; }

        [Display(Name = "Nhân viên")]
        public User User { get; set; }

        public int CustomerID { get; set; }

        [Display(Name = "Khách hàng")]
        public Customer Customer { get; set; }

        public List<DetailOrder> DetailOrders { get; set; }
    }
}
