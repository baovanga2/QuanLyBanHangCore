using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class CountByProductDateViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập thời gian bắt đầu!")]
        [DataType(DataType.Date)]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime Dau { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thời gian kết thúc!")]
        [DataType(DataType.Date)]
        [Display(Name = "Thời gian kết thúc")]
        [Remote(action:"KiemTraNgayCuoi", controller:"Statistics", AdditionalFields ="Dau")]
        public DateTime Cuoi { get; set; }
        public List<ProductCount> Products { get; set; }
        public CountByProductDateViewModel()
        {
            Products = new List<ProductCount>();
        }
        public ulong LayTongTien()
        {
            ulong tongTien = 0;
            foreach (var p in Products)
            {
                tongTien += p.LayTamTinh();
            }
            return tongTien;
        }
    }

    public  class ProductCount
    {
        [Display(Name = "Tên sản phẩm")]
        public string Ten { get; set; }
        [Display(Name = "Giá")]
        public ulong Gia { get; set; }
        [Display(Name = "Số lượng")]
        public ushort SoLuong { get; set; }
        public ulong LayTamTinh()
        {
            return Gia * SoLuong;
        }
    }
}
