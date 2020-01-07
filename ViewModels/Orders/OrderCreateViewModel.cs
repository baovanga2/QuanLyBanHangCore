using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class OrderCreateViewModel
    {
        public OrderCreateViewModel()
        {
            DetailOrders = new List<ItemCreateViewModel>();
            Products = new List<ProductWithCurrentPrice>();
            ThoiGianTao = DateTime.Now;
        }

        public DateTime ThoiGianTao { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [Display(Name = "Khách hàng")]
        public int CustomerID { get; set; }
        public List<ProductWithCurrentPrice> Products { get; set; }
        public List<ItemCreateViewModel> DetailOrders { get; set; }

        public ulong LayTongTien()
        {
            ulong tongTien = 0;
            foreach (var item in DetailOrders)
            {
                tongTien += item.LayTamTinh();
            }
            return tongTien;
        }
    }

    public class ItemCreateViewModel
    {
        public int ProductID { get; set; }
        public string ProductTen { get; set; }
        public ulong Gia { get; set; }
        public uint SoLuongBan { get; set; }

        public ulong LayTamTinh()
        {
            return Gia * SoLuongBan;
        }
    }
}