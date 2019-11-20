using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Models.ViewModels
{
    public class HomeViewModel
    {
        public double TongTienNgay { get; set; }
        public double TongTienThang { get; set; }

        public List<ChartBarViewModel> MyProperty { get; set; }
    }
}
