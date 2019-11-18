﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class OrderCreateViewModel
    {
        public OrderCreateViewModel()
        {
            DetailOrders = new List<ItemCreateViewModel>();
        }
        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        public int? CustomerID { get; set; }
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
        public ushort SoLuongBan { get; set; }
        public ulong LayTamTinh()
        {
            return Gia * SoLuongBan;
        }
    }
}
