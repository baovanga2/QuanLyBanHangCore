using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;

        public HomeController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.DetailOrders)
                .AsNoTracking()
                .ToListAsync();
            var ordersNgay = orders.Where(o => o.ThoiGianTao.Date == DateTime.Today).ToList();
            double tongTienNgay = 0;
            foreach (var o in ordersNgay)
            {
                foreach (var i in o.DetailOrders)
                {
                    tongTienNgay += i.Gia * i.SoLuong;
                }
            }
            var ordersThang = orders
                .Where(o => o.ThoiGianTao.Month == DateTime.Today.Month
                    && o.ThoiGianTao.Year == DateTime.Today.Year).ToList();
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
                TongTienThang = tongTienThang,
                SoDonHangNgay = (uint)ordersNgay.Count()
            };
            string[] months = { "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín",
                "Mười", "Mười một", "Mười hai"};
            for (int i = 1; i <= 12; i++)
            {
                var ordersThangNay = orders
                .Where(o => o.ThoiGianTao.Month == i
                    && o.ThoiGianTao.Year == DateTime.Today.Year);
                double tongTienThangNay = 0;
                foreach (var o in ordersThangNay)
                {
                    foreach (var item in o.DetailOrders)
                    {
                        tongTienThangNay += item.Gia * item.SoLuong;
                    }
                }
                var tongTienHangThang = new ChartBarViewModel
                {
                    Thang = months[i - 1],
                    ThuNhap = tongTienThangNay
                };
                model.TongTienHangThang.Add(tongTienHangThang);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> ViewOrderChange()
        {
            return View(await _context.OrderChanges.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> ViewUserChange()
        {
            return View(await _context.UserChanges.AsNoTracking().ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}