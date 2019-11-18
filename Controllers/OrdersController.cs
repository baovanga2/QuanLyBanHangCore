using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Helpers;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;

namespace QuanLyBanHangCore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;
        private readonly UserManager<User> _userManager;
        public OrdersController(QuanLyBanHangCoreContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị,Bán hàng,Kế toán,Thủ kho")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.User).Include(o => o.Customer).Include(o => o.DetailOrders).AsNoTracking().ToListAsync();
            var model = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var orderVM = new OrderViewModel
                {
                    ID = order.ID,
                    ThoiGianTao = order.ThoiGianTao,
                    UserName = order.User.Ten,
                    CustomerName = order.Customer.Ten,
                    DetailOrders = order.DetailOrders
                };
                model.Add(orderVM);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị,Bán hàng,Kế toán,Thủ kho")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include(o => o.User).Include(o => o.Customer).Include(o => o.DetailOrders).ThenInclude(i => i.Product).AsNoTracking().FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDetailsViewModel
            {
                ID = order.ID,
                ThoiGianTao = order.ThoiGianTao,
                UserName = order.User.Ten,
                CustomerName = order.Customer.Ten
            };
            foreach (var item in order.DetailOrders)
            {
                var itemVM = new ItemDetailsViewModel
                {
                    Gia = item.Gia,
                    SoLuong = item.SoLuong,
                    Ten = item.Product.Ten
                };
                model.Items.Add(itemVM);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public async Task<IActionResult> Create()
        {
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            if (order == null)
            {
                order = new OrderCreateViewModel();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
            }
            var now = SessionHelper.GetObjectFormJson<DateTime>(HttpContext.Session, "now");
            if (now == default(DateTime))
            {
                now = DateTime.Now;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "now", now);
            }
            var productWithCurrentPrices = new List<ProductWithCurrentPrice>();
            var products = await _context.Products.AsNoTracking().ToListAsync();
            foreach (Product p in products)
            {
                var productPrice = await _context.ProductPrices.AsNoTracking().FirstOrDefaultAsync(pp => pp.ProductID == p.ID && pp.TGKT > now && pp.TGBD <= now);
                var productWithCurrentPrice = new ProductWithCurrentPrice
                {
                    ID = p.ID,
                    Ten = p.Ten,
                    SoLuong = p.SoLuong,
                    Gia = productPrice.Gia
                };
                productWithCurrentPrices.Add(productWithCurrentPrice);
            }
            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.ID == order.CustomerID);
            if (customer == null)
            {
                ViewBag.CustomerID = new SelectList(_context.Customers, "ID", "Ten");
            }
            else
            {
                ViewBag.CustomerID = new SelectList(_context.Customers, "ID", "Ten", customer.ID);
            }
            ViewBag.Order = order;
            ViewBag.Products = productWithCurrentPrices;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Bán hàng")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (OrderCreateViewModel model)
        {
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            if (order.DetailOrders.Count==0)
            {
                ModelState.AddModelError("", "Vui lòng thêm sản phẩm vào đơn hàng!");
            }
            var now = SessionHelper.GetObjectFormJson<DateTime>(HttpContext.Session, "now");
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.ID == model.CustomerID);
                if (customer == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    RedirectToAction("Login", "Users");
                }
                var listProduct = _context.Products;
                var orderNew = new Order
                {
                    ThoiGianTao = now,
                    UserID = user.Id,
                    CustomerID = customer.ID
                };
                _context.Add(orderNew);
                await _context.SaveChangesAsync();
                foreach (var item in order.DetailOrders)
                {
                    var detailOrder = new DetailOrder
                    {
                        Gia = item.Gia,
                        SoLuong = item.SoLuongBan,
                        OrderID = orderNew.ID,
                        ProductID = item.ProductID
                    };
                    _context.Add(detailOrder);
                    _context.SaveChanges();
                    var productEdit = await listProduct.FirstOrDefaultAsync(p => p.ID == item.ProductID);
                    productEdit.SoLuong -= item.SoLuongBan;
                    _context.Products.Update(productEdit);
                    await _context.SaveChangesAsync();
                }
                TempData["messageSuccess"] = $"Đơn hàng \"{orderNew.ID}\" đã thêm";
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
            var productWithCurrentPrices = new List<ProductWithCurrentPrice>();
            var products = await _context.Products.AsNoTracking().ToListAsync();
            foreach (Product p in products)
            {
                var productPrice = await _context.ProductPrices.AsNoTracking().FirstOrDefaultAsync(pp => pp.ProductID == p.ID && pp.TGKT > now && pp.TGBD <= now);
                var productWithCurrentPrice = new ProductWithCurrentPrice
                {
                    ID = p.ID,
                    Ten = p.Ten,
                    SoLuong = p.SoLuong,
                    Gia = productPrice.Gia
                };
                productWithCurrentPrices.Add(productWithCurrentPrice);
            }
            ViewBag.CustomerID = new SelectList(_context.Customers, "ID", "Ten", model.CustomerID);
            ViewBag.Order = order;
            ViewBag.Products = productWithCurrentPrices;
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public async Task<IActionResult> InHoaDon(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders).ThenInclude(i => i.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            var model = new BillViewModel
            {
                ID = order.ID,
                ThoiGianTao = order.ThoiGianTao,
                CustomerName = order.Customer.Ten,
                CustomerSDT = order.Customer.SDT ?? "#### ### ###",
                UserName = order.User.Ten
            };
            foreach (var item in order.DetailOrders)
            {
                var itemVM = new ItemOfBillViewModel
                {
                    Gia = item.Gia,
                    SoLuong = item.SoLuong,
                    Ten = item.Product.Ten
                };
                model.Items.Add(itemVM);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var order = await _context.Orders.Include(o => o.User).Include(o => o.Customer).Include(o => o.DetailOrders).ThenInclude(i => i.Product).AsNoTracking().FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderDetailsViewModel
            {
                ID = order.ID,
                ThoiGianTao = order.ThoiGianTao,
                UserName = order.User.Ten,
                CustomerName = order.Customer.Ten
            };
            foreach (var item in order.DetailOrders)
            {
                var itemVM = new ItemDetailsViewModel
                {
                    Gia = item.Gia,
                    SoLuong = item.SoLuong,
                    Ten = item.Product.Ten
                };
                model.Items.Add(itemVM);
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Bán hàng")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order =await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["messageSuccess"] = $"Đơn hàng \"{order.ID}\" đã xóa";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public IActionResult LoadCart()
        {
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            return PartialView("_CartAjax", order);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public IActionResult Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            var now = SessionHelper.GetObjectFormJson<DateTime>(HttpContext.Session, "now");
            var productPrice = _context.ProductPrices.AsNoTracking().FirstOrDefault(pp => pp.ProductID == id && pp.TGKT > now && pp.TGBD <= now);
            var item = new ItemCreateViewModel
            {
                ProductID = product.ID,
                ProductTen = product.Ten,
                Gia = productPrice.Gia,
                SoLuongBan = 1
            };
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            int index = isExist(id);
            if (index != -1)
            {
                order.DetailOrders[index].SoLuongBan++;
            }
            else
            {
                order.DetailOrders.Add(item);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
            return PartialView("_CartAjax", order);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public IActionResult Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int index = isExist(id);
            if (index == -1)
            {
                return NotFound();
            }
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            order.DetailOrders.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
            return PartialView("_CartAjax", order);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        public IActionResult ThayDoiSoLuongBan(int? id, ushort soLuong = 0)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            if (order.DetailOrders.FirstOrDefault(dd => dd.ProductID == id) == null)
            {
                return NotFound();
            }
            if (soLuong > product.SoLuong)
            {
                order.DetailOrders.FirstOrDefault(dd => dd.ProductID == id).SoLuongBan = product.SoLuong;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
                return PartialView("_CartAjax", order);
            }
            if (soLuong <= 0)
            {
                order.DetailOrders.FirstOrDefault(dd => dd.ProductID == id).SoLuongBan = 1;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
                return PartialView("_CartAjax", order);
            }
            order.DetailOrders.FirstOrDefault(dd => dd.ProductID == id).SoLuongBan = soLuong;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
            return PartialView("_CartAjax", order);
        }

        [HttpGet]
        [Authorize(Roles = "Bán hàng")]
        private int isExist(int? id)
        {
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            for (int i = 0; i < order.DetailOrders.Count; i++)
            {
                if (order.DetailOrders[i].ProductID == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}