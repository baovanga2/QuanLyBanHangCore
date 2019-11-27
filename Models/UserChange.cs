using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHangCore.Models
{
    public class UserChange
    {
        public int ID { get; set; }

        [Display(Name = "Người thay đổi")]
        public string NguoiCapNhat { get; set; }
        [Display(Name = "Người bị thay đổi")]
        public string NguoiBiCapNhat { get; set; }
        [Display(Name = "Hành động")]
        public string HanhDong { get; set; }
        [Display(Name = "Thời gian")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime ThoiGian { get; set; }
    }
}