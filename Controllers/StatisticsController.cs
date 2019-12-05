using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;

        public StatisticsController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Statistics/Index")]
        [Authorize(Roles = "Kế toán,Thủ kho")]
        public async Task<IActionResult> CountByProductDate()
        {
            var now = DateTime.Today;
            var model = new CountByProductDateViewModel
            {
                Dau = now,
                Cuoi = now,
                Products = await GetProductCountsByDateAsync(now, now)
            };
            return View(model);
        }

        [HttpPost]
        [Route("Statistics/Index")]
        [Authorize(Roles = "Kế toán, Thủ kho")]
        public async Task<IActionResult> CountByProductDate(CountByProductDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Products = await GetProductCountsByDateAsync(model.Dau, model.Cuoi);
            }
            return View(model);
        }

        private async Task<List<ProductCount>> GetProductCountsByDateAsync(DateTime dau, DateTime cuoi)
        {
            var orders = await _context.Orders
                    .Include(o => o.DetailOrders)
                        .ThenInclude(dd => dd.Product)
                    .AsNoTracking()
                    .Where(o => dau.Date <= o.ThoiGianTao.Date
                        && o.ThoiGianTao.Date <= cuoi.Date)
                    .ToListAsync();
            var items = new List<ProductCount>();
            foreach (var order in orders)
            {
                foreach (var i in order.DetailOrders)
                {
                    var item = new ProductCount
                    {
                        Ten = i.Product.Ten,
                        Gia = i.Gia,
                        SoLuong = i.SoLuong
                    };
                    items.Add(item);
                }
            }
            var itemsGroup = items.GroupBy(i => new { i.Ten, i.Gia })
                .Select(i => new
                {
                    Ten = i.Key.Ten,
                    Gia = i.Key.Gia,
                    SoLuong = (uint)i.Sum(c => c.SoLuong)
                });
            var products = new List<ProductCount>();
            foreach (var i in itemsGroup)
            {
                var product = new ProductCount
                {
                    Ten = i.Ten,
                    Gia = i.Gia,
                    SoLuong = i.SoLuong
                };
                products.Add(product);
            }
            return products;
        }

        [HttpGet]
        [Authorize(Roles = "Kế toán, Thủ kho")]
        public async Task<IActionResult> CountByProductMonth()
        {
            var nowm = DateTime.Today.Month;
            var nowy = DateTime.Today.Year;
            var model = new CountByProductMonthViewModel
            {
                Thang = nowm,
                Nam = nowy,
                Products = await GetProductCountsByMonthAsync(nowy, nowm)
            };
            ViewBag.Months = new SelectList(model.Months, "ID", "Ten", nowm);
            ViewBag.Years = new SelectList(model.Years, nowy);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Kế toán,Thủ kho")]
        public async Task<IActionResult> CountByProductMonth(CountByProductMonthViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Products = await GetProductCountsByMonthAsync(model.Nam, model.Thang);
                ViewBag.Months = new SelectList(model.Months, "ID", "Ten", model.Thang);
                ViewBag.Years = new SelectList(model.Years, model.Nam);
            }
            return View(model);
        }

        private async Task<List<ProductCount>> GetProductCountsByMonthAsync(int nam, int thang)
        {
            var orders = await _context.Orders
                        .Include(o => o.DetailOrders)
                            .ThenInclude(dd => dd.Product)
                        .AsNoTracking()
                        .Where(o => o.ThoiGianTao.Year == nam)
                        .ToListAsync();
            if (thang != 0)
            {
                orders = orders.Where(o => o.ThoiGianTao.Month == thang).ToList();
            }
            var items = new List<ProductCount>();
            foreach (var order in orders)
            {
                foreach (var i in order.DetailOrders)
                {
                    var item = new ProductCount
                    {
                        Ten = i.Product.Ten,
                        Gia = i.Gia,
                        SoLuong = i.SoLuong
                    };
                    items.Add(item);
                }
            }
            var itemsGroup = items.GroupBy(p => new { p.Ten, p.Gia })
                .Select(p => new
                {
                    Ten = p.Key.Ten,
                    Gia = p.Key.Gia,
                    SoLuong = (ushort)p.Sum(s => s.SoLuong)
                });
            var products = new List<ProductCount>();
            foreach (var i in itemsGroup)
            {
                var product = new ProductCount
                {
                    Ten = i.Ten,
                    Gia = i.Gia,
                    SoLuong = i.SoLuong
                };
                products.Add(product);
            }
            return products;
        }

        [HttpGet]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> CountByCustomerDate()
        {
            var now = DateTime.Today;
            var model = new CountByCustomerDateViewModel
            {
                Dau = now,
                Cuoi = now,
                Customers = await GetCustomerCountsByDateAsync(now, now)
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> CountByCustomerDate(CountByCustomerDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Customers = await GetCustomerCountsByDateAsync(model.Dau, model.Cuoi);
            }
            return View(model);
        }

        private async Task<List<CustomerCount>> GetCustomerCountsByDateAsync(DateTime dau, DateTime cuoi)
        {
            var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.DetailOrders)
                        .ThenInclude(dd => dd.Product)
                    .AsNoTracking()
                    .Where(o => dau.Date <= o.ThoiGianTao.Date && o.ThoiGianTao.Date <= cuoi.Date)
                    .ToListAsync();
            var items = new List<CustomerCount>();
            foreach (var order in orders)
            {
                ulong tongTien = 0;
                foreach (var i in order.DetailOrders)
                {
                    var tamTinh = i.Gia * i.SoLuong;
                    tongTien += tamTinh;
                }
                var item = new CustomerCount
                {
                    ID = order.CustomerID,
                    Ten = order.Customer.Ten,
                    Tien = tongTien
                };
                items.Add(item);
            }
            var itemsGroup = items.GroupBy(i => new { i.ID, i.Ten }).Select(i => new
            {
                ID = i.Key.ID,
                Ten = i.Key.Ten,
                Tien = i.Sum(i => i.Tien)
            });
            var customers = new List<CustomerCount>();
            foreach (var i in itemsGroup)
            {
                var customer = new CustomerCount
                {
                    ID = i.ID,
                    Ten = i.Ten,
                    Tien = i.Tien
                };
                customers.Add(customer);
            }
            return customers;
        }

        [HttpGet]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> CountByCustomerMonth()
        {
            var nowm = DateTime.Today.Month;
            var nowy = DateTime.Today.Year;
            var model = new CountByCustomerMonthViewModel
            {
                Thang = nowm,
                Nam = nowy,
                Customers = await GetCustomerCountsByMonthAsync(nowy, nowm)
            };
            ViewBag.Months = new SelectList(model.Months, "ID", "Ten", nowm);
            ViewBag.Years = new SelectList(model.Years, nowy);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> CountByCustomerMonth(CountByCustomerMonthViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Customers = await GetCustomerCountsByMonthAsync(model.Nam, model.Thang);
                ViewBag.Months = new SelectList(model.Months, "ID", "Ten", model.Thang);
                ViewBag.Years = new SelectList(model.Years, model.Nam);
            }
            return View(model);
        }

        private async Task<List<CustomerCount>> GetCustomerCountsByMonthAsync(int nam, int thang)
        {
            var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.DetailOrders)
                        .ThenInclude(dd => dd.Product)
                    .AsNoTracking()
                    .Where(o => o.ThoiGianTao.Year == nam)
                    .ToListAsync();
            if (thang != 0)
            {
                orders = orders.Where(o => o.ThoiGianTao.Month == thang).ToList();
            }
            var items = new List<CustomerCount>();
            foreach (var order in orders)
            {
                ulong tongTien = 0;
                foreach (var i in order.DetailOrders)
                {
                    var tamTinh = i.Gia * i.SoLuong;
                    tongTien += tamTinh;
                }
                var item = new CustomerCount
                {
                    ID = order.CustomerID,
                    Ten = order.Customer.Ten,
                    Tien = tongTien
                };
                items.Add(item);
            }
            var itemsGroup = items.GroupBy(i => new { i.ID, i.Ten }).Select(i => new
            {
                ID = i.Key.ID,
                Ten = i.Key.Ten,
                Tien = i.Sum(i => i.Tien)
            });
            var customers = new List<CustomerCount>();
            foreach (var i in itemsGroup)
            {
                var customer = new CustomerCount
                {
                    ID = i.ID,
                    Ten = i.Ten,
                    Tien = i.Tien
                };
                customers.Add(customer);
            }
            return customers;
        }

        [HttpPost]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> ExportByDate(CountByProductDateViewModel model)
        {
            model.Products = await GetProductCountsByDateAsync(model.Dau, model.Cuoi);
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var options = SaveOptions.XlsxDefault;
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            worksheet.Cells[0, 0].Value = "Các sản phẩm đã bán từ " + DateToString(model.Dau) +
                " đến " + DateToString(model.Cuoi);
            worksheet.Cells[0, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Cells.GetSubrangeAbsolute(0, 0, 0, 3).Merged = true;

            worksheet.Rows[0].Style.Font.Weight = ExcelFont.BoldWeight;
            worksheet.Rows[1].Style.Font.Weight = ExcelFont.BoldWeight;
            worksheet.Columns[2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            worksheet.Columns[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

            worksheet.Columns[0].SetWidth(500, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(80, LengthUnit.Pixel);
            worksheet.Columns[3].SetWidth(150, LengthUnit.Pixel);

            worksheet.Cells["A2"].Value = "Tên sản phẩm";
            worksheet.Cells["B2"].Value = "Giá";
            worksheet.Cells["C2"].Value = "Số lượng";
            worksheet.Cells["D2"].Value = "Tạm tính";
            var l = model.Products.Count;
            for (int r = 1; r < l; r++)
            {
                var product = model.Products[r - 1];
                worksheet.Cells[r+1, 0].Value = product.Ten;
                worksheet.Cells[r+1, 1].Value = String.Format("{0:### ### ### ### VND}", product.Gia);
                worksheet.Cells[r+1, 2].Value = product.SoLuong;
                worksheet.Cells[r+1, 3].Value = String.Format("{0:### ### ### ### VND}", product.LayTamTinh());
            }

            worksheet.Cells[l + 2, 2].Value = "Tổng tiền:";
            worksheet.Cells[l + 2, 3].Value = String.Format("{0:### ### ### ### VND}", model.LayTongTien());

            return File(GetBytes(workbook, options), options.ContentType, "Thống kê từ " +
                DateToString(model.Dau) + " đến " + DateToString(model.Cuoi) + ".xlsx");
        }

        private string DateToString(DateTime date)
        {
            return date.Day + "-" + date.Month + "-" + date.Year;
        }

        private static byte[] GetBytes(ExcelFile file, SaveOptions options)
        {
            using (var stream = new MemoryStream())
            {
                file.Save(stream, options);
                return stream.ToArray();
            }
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult KiemTraNgayCuoi(DateTime cuoi, DateTime dau)
        {
            if (cuoi.Date >= dau.Date)
            {
                return Json(true);
            }
            return Json("Thời gian kết thúc không được sớm hơn thời gian bắt đầu!");
        }
    }
}