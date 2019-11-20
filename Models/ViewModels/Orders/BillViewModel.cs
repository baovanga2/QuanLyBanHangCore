using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class BillViewModel
    {
        public BillViewModel()
        {
            Items = new List<ItemOfBillViewModel>();
        }
        [Display(Name = "Mã đơn hàng")]
        public int ID { get; set; }
        [Display(Name = "Thời gian")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ThoiGianTao { get; set; }
        [Display(Name = "Nhân viên bán")]
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSDT { get; set; }
        public List<ItemOfBillViewModel> Items { get; set; }
        public ulong LayTongTien()
        {
            ulong tongTien = 0;
            foreach (var item in Items)
            {
                tongTien += item.LayTamTinh();
            }
            return tongTien;
        }
    }
    public class ItemOfBillViewModel
    {
        public ulong Gia { get; set; }
        public uint SoLuong { get; set; }
        public string Ten { get; set; }
        public ulong LayTamTinh()
        {
            return Gia * SoLuong;
        }
    }
}
