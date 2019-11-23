using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            DetailOrders = new List<DetailOrder>();
        }

        [Display(Name = "Mã đơn")]
        public int ID { get; set; }

        [Display(Name = "Thời gian")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime ThoiGianTao { get; set; }

        [Display(Name = "Nhân viên")]
        public string UserName { get; set; }

        [Display(Name = "Khách hàng")]
        public string CustomerName { get; set; }

        [Display(Name = "Danh sách sản phẩm")]
        public List<DetailOrder> DetailOrders { get; set; }

        public ulong LayTongTien()
        {
            ulong tongTien = 0;
            foreach (var item in DetailOrders)
            {
                tongTien += item.Gia * item.SoLuong;
            }
            return tongTien;
        }
    }
}