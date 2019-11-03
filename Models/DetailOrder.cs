using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models
{
    public class DetailOrder
    {
        [Display(Name = "Giá")]
        public ulong Gia { get; set; }

        [Display(Name = "Số lượng")]
        public ushort SoLuong { get; set; }

        public int OrderID { get; set; }

        [Display(Name = "Đơn hàng")]
        public Order Order { get; set; }

        [Display(Name = "Sản phẩm")]
        public int ProductID { get; set; }

        public Product Product { get; set; }
    }
}
