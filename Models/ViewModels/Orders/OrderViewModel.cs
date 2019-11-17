using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "Mã")]
        public int ID { get; set; }
        [Display(Name = "Thời gian")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ThoiGianTao { get; set; }
        [Display(Name = "Nhân viên")]
        public string UserName { get; set; }
        [Display(Name = "Khách hàng")]
        public string CustomerName { get; set; }
        public List<DetailOrder> DetailOrders { get; set; }

        public ulong LayThanhTien()
        {
            ulong tongTien = 0;
            foreach (var item in DetailOrders)
            {
                tongTien += item.Gia * item.SoLuong;
            }
            tongTien = tongTien * 110 / 100;
            return tongTien;
        }
    }
}
