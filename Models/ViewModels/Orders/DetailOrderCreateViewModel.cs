using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class DetailOrderCreateViewModel
    {
        public int ProductID { get; set; }
        public string   ProductTen { get; set; }
        public ulong Gia { get; set; }
        public ushort SoLuongBan { get; set; }
        public ulong LayTamTinh()
        {
            return Gia * SoLuongBan;
        }
    }
}
