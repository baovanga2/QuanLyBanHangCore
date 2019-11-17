using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            Items = new List<ItemDetailsViewModel>();
        }
        [Display(Name = "Mã đơn hàng")]
        public int ID { get; set; }
        [Display(Name = "Thời gian")]
        public DateTime ThoiGianTao { get; set; }
        [Display(Name = "Nhân viên bán")]
        public string UserName { get; set; }
        [Display(Name = "Khách hàng")]
        public string CustomerName { get; set; }
        [Display(Name = "Danh sách sản phẩm")]
        public List<ItemDetailsViewModel> Items { get; set; }
        public ulong LayThanhTien()
        {
            ulong tongTien = 0;
            foreach (var item in Items)
            {
                tongTien += item.LayTamTinh();
            }
            return tongTien * 110 / 100;
        }
    }
    public class ItemDetailsViewModel
    {
        public ulong Gia { get; set; }
        public ushort SoLuong { get; set; }
        public string Ten { get; set; }
        public ulong LayTamTinh()
        {
            return Gia * SoLuong;
        }
    }
}
