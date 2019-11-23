using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;

namespace QuanLyBanHangCore.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;
        public StatisticsController (QuanLyBanHangCoreContext context)
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
                Cuoi = now
            };
            var orders = await _context.Orders
                .Include(o => o.DetailOrders)
                    .ThenInclude(dd => dd.Product)
                .AsNoTracking()
                .Where(o => o.ThoiGianTao.Date == now)
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
            var itemsGroup = items.GroupBy(p => new { p.Ten, p.Gia })
                .Select(p => new { Ten = p.Key.Ten, Gia = p.Key.Gia,
                    SoLuong = (ushort)p.Sum(s => s.SoLuong)});
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
            model.Products = products;
            return View(model);
        }

        [HttpPost]
        [Route("Statistics/Index")]
        [Authorize(Roles = "Kế toán, Thủ kho")]
        public async Task<IActionResult> CountByProductDate(CountByProductDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orders = await _context.Orders
                    .Include(o => o.DetailOrders)
                        .ThenInclude(dd => dd.Product)
                    .AsNoTracking()
                    .Where(o => model.Dau.Date <= o.ThoiGianTao.Date
                        && o.ThoiGianTao.Date <= model.Cuoi.Date)
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
                    .Select(i => new { Ten = i.Key.Ten, Gia = i.Key.Gia, SoLuong = (ushort)i.Sum(c => c.SoLuong) });
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
                model.Products = products;
            }
            return View(model);
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
                Nam = nowy
            };
            var orders = await _context.Orders
                .Include(o => o.DetailOrders)
                    .ThenInclude(dd => dd.Product)
                .AsNoTracking()
                .Where(o => o.ThoiGianTao.Month == nowm && o.ThoiGianTao.Year == nowy)
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
            var itemsGroup = items.GroupBy(p => new { p.Ten, p.Gia })
                .Select(p => new { Ten = p.Key.Ten, Gia = p.Key.Gia,
                    SoLuong = (ushort)p.Sum(s => s.SoLuong) });
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
            model.Products = products;
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
                var orders = new List<Order>();
                if (model.Thang == 0)
                {
                    orders = await _context.Orders
                        .Include(o => o.DetailOrders)
                            .ThenInclude(dd => dd.Product)
                        .AsNoTracking()
                        .Where(o => o.ThoiGianTao.Year == model.Nam)
                        .ToListAsync();
                }
                else
                {
                    orders = await _context.Orders
                        .Include(o => o.DetailOrders)
                            .ThenInclude(dd => dd.Product)
                        .AsNoTracking()
                        .Where(o => o.ThoiGianTao.Month == model.Thang
                            && o.ThoiGianTao.Year == model.Nam)
                        .ToListAsync();
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
                    .Select(p => new { Ten = p.Key.Ten, Gia = p.Key.Gia,
                        SoLuong = (ushort)p.Sum(s => s.SoLuong) });
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
                model.Products = products;
                ViewBag.Months = new SelectList(model.Months, "ID", "Ten", model.Thang);
                ViewBag.Years = new SelectList(model.Years, model.Nam);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> CountByCustomerDate()
        {
            var now = DateTime.Today;
            var model = new CountByCustomerDateViewModel
            {
                Dau = now,
                Cuoi = now
            };
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                    .ThenInclude(dd => dd.Product)
                .AsNoTracking()
                .Where(o => o.ThoiGianTao.Date == now)
                .ToListAsync();
            var items = new List<CustomerCount>();
            foreach (var order in orders)
            {
                var item = new CustomerCount
                {
                    ID = order.CustomerID,
                    Ten = order.Customer.Ten
                };
                ulong tongTien = 0;
                foreach (var i in order.DetailOrders)
                {
                    var tamTinh = i.Gia * i.SoLuong;
                    tongTien += tamTinh;
                }
                item.Tien = tongTien;
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
            model.Customers = customers;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Kế toán")]
        public async Task<IActionResult> CountByCustomerDate(CountByCustomerDateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.DetailOrders)
                        .ThenInclude(dd => dd.Product)
                    .AsNoTracking()
                    .Where(o => model.Dau.Date <= o.ThoiGianTao.Date && o.ThoiGianTao.Date <= model.Cuoi.Date)
                    .ToListAsync();
                var items = new List<CustomerCount>();
                foreach (var order in orders)
                {
                    var item = new CustomerCount
                    {
                        ID = order.CustomerID,
                        Ten = order.Customer.Ten
                    };
                    ulong tongTien = 0;
                    foreach (var i in order.DetailOrders)
                    {
                        var tamTinh = i.Gia * i.SoLuong;
                        tongTien += tamTinh;
                    }
                    item.Tien = tongTien;
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
                model.Customers = customers;
            }
            return View(model);
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
                Nam = nowy
            };
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                    .ThenInclude(dd => dd.Product)
                .AsNoTracking()
                .Where(o => o.ThoiGianTao.Month == nowm && o.ThoiGianTao.Year == nowy)
                .ToListAsync();
            var items = new List<CustomerCount>();
            foreach (var order in orders)
            {
                var item = new CustomerCount
                {
                    ID = order.CustomerID,
                    Ten = order.Customer.Ten
                };
                ulong tongTien = 0;
                foreach (var i in order.DetailOrders)
                {
                    var tamTinh = i.Gia * i.SoLuong;
                    tongTien += tamTinh;
                }
                item.Tien = tongTien;
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
            model.Customers = customers;
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
                var orders = new List<Order>();
                if (model.Thang == 0)
                {
                    orders = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.DetailOrders)
                            .ThenInclude(dd => dd.Product)
                        .AsNoTracking()
                        .Where(o => o.ThoiGianTao.Year == model.Nam)
                        .ToListAsync();
                }
                else
                {
                    orders = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.DetailOrders)
                             .ThenInclude(dd => dd.Product)
                        .AsNoTracking()
                        .Where(o => o.ThoiGianTao.Month == model.Thang && o.ThoiGianTao.Year == model.Nam)
                        .ToListAsync();
                }
                var items = new List<CustomerCount>();
                foreach (var order in orders)
                {
                    var item = new CustomerCount
                    {
                        ID = order.CustomerID,
                        Ten = order.Customer.Ten
                    };
                    ulong tongTien = 0;
                    foreach (var i in order.DetailOrders)
                    {
                        var tamTinh = i.Gia * i.SoLuong;
                        tongTien += tamTinh;
                    }
                    item.Tien = tongTien;
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
                model.Customers = customers;
                ViewBag.Months = new SelectList(model.Months, "ID", "Ten", model.Thang);
                ViewBag.Years = new SelectList(model.Years, model.Nam);
            }
            return View(model);
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