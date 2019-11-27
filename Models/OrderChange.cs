using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class OrderChange
    {
        public int ID { get; set; }

        [Display(Name = "Người dùng")]
        public string UserName { get; set; }
        [Display(Name = "Mã đơn hàng")]
        public int OrderID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        [Display(Name = "Thời gian")]
        public DateTime ThoiGian { get; set; }
        [Display(Name = "Hành động")]
        public string HanhDong { get; set; }
    }
}