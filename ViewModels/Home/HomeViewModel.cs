using System.Collections.Generic;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            TongTienHangThang = new List<ChartBarViewModel>();
        }
        public double TongTienNgay { get; set; }
        public double TongTienThang { get; set; }
        public uint SoDonHangNgay { get; set; }

        public List<ChartBarViewModel> TongTienHangThang { get; set; }
    }
}