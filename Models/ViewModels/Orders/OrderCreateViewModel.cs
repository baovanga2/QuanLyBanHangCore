using Microsoft.AspNetCore.Mvc;
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
            DetailOrders = new List<DetailOrderCreateViewModel>();
        }
        public List<DetailOrderCreateViewModel> DetailOrders { get; set; }

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
}
