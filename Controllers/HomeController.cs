using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;

namespace QuanLyBanHangCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;
        public HomeController (QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.DetailOrders)
                .AsNoTracking().ToListAsync();
            var ordersNgay = orders.Where(o => o.ThoiGianTao == DateTime.Today);
            double tongTienNgay = 0;
            foreach (var o in ordersNgay)
            {
                foreach (var i in o.DetailOrders)
                {
                    tongTienNgay += i.Gia * i.SoLuong;
                }
            }
            var ordersThang = orders
                .Where(o => o.ThoiGianTao.Month == DateTime.Today.Month && o.ThoiGianTao.Year == DateTime.Today.Year);
            double tongTienThang = 0;
            foreach (var o in ordersThang)
            {
                foreach (var i in o.DetailOrders)
                {
                    tongTienThang += i.Gia * i.SoLuong;
                }
            }
            var model = new HomeViewModel
            {
                TongTienNgay = tongTienNgay,
                TongTienThang = tongTienThang
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
