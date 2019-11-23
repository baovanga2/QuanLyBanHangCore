using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Helpers;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [Authorize(Roles = "Quản trị, Thu ngân, Kế toán, Thủ kho")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                .AsNoTracking()
                .ToListAsync();
            var model = new List<OrderViewModel>();
            foreach (var o in orders)
            {
                var order = new OrderViewModel
                {
                    ID = o.ID,
                    ThoiGianTao = o.ThoiGianTao,
                    UserName = o.User.Ten,
                    CustomerName = o.Customer.Ten,
                    DetailOrders = o.DetailOrders
                };
                model.Add(order);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Quản trị, Thu ngân, Kế toán, Thủ kho")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                    .ThenInclude(i => i.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderViewModel
            {
                ID = order.ID,
                ThoiGianTao = order.ThoiGianTao,
                UserName = order.User.Ten,
                CustomerName = order.Customer.Ten
            };
            foreach (var item in order.DetailOrders)
            {
                model.DetailOrders.Add(item);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> Create()
        {
            var order = SessionHelper
                .GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            if (order == null)
            {
                order = new OrderCreateViewModel();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
            }
            var productWithCurrentPrices = new List<ProductWithCurrentPrice>();
            var products = await _context.Products.AsNoTracking().ToListAsync();
            foreach (Product p in products)
            {
                var productPrice = await _context.ProductPrices
                    .AsNoTracking()
                    .FirstOrDefaultAsync(pp => pp.ProductID == p.ID
                        && pp.TGKT > order.ThoiGianTao && pp.TGBD <= order.ThoiGianTao);
                var productWithCurrentPrice = new ProductWithCurrentPrice
                {
                    ID = p.ID,
                    Ten = p.Ten,
                    SoLuong = p.SoLuong,
                    Gia = productPrice.Gia
                };
                productWithCurrentPrices.Add(productWithCurrentPrice);
            }
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ID == order.CustomerID);
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
        [Authorize(Roles = "Thu ngân")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID")] OrderCreateViewModel model)
        {
            var order = SessionHelper
                .GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            if (order.DetailOrders.Count == 0)
            {
                ModelState.AddModelError("", "Vui lòng thêm sản phẩm vào đơn hàng!");
            }
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.ID == model.CustomerID);
                if (customer == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    RedirectToAction("Login", "Users");
                }
                var orderAdd = new Order
                {
                    ThoiGianTao = order.ThoiGianTao,
                    UserID = user.Id,
                    CustomerID = customer.ID
                };
                _context.Add(orderAdd);
                await _context.SaveChangesAsync();
                foreach (var i in order.DetailOrders)
                {
                    var detailOrder = new DetailOrder
                    {
                        Gia = i.Gia,
                        SoLuong = i.SoLuongBan,
                        OrderID = orderAdd.ID,
                        ProductID = i.ProductID
                    };
                    _context.Add(detailOrder);
                    _context.SaveChanges();
                    var productEdit = await _context.Products
                        .FirstOrDefaultAsync(p => p.ID == i.ProductID);
                    productEdit.SoLuong -= i.SoLuongBan;
                    _context.Products.Update(productEdit);
                    await _context.SaveChangesAsync();
                }
                TempData["messageSuccess"] = $"Đơn hàng \"{orderAdd.ID}\" đã thêm";
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
            var productWithCurrentPrices = new List<ProductWithCurrentPrice>();
            var products = await _context.Products.AsNoTracking().ToListAsync();
            foreach (Product p in products)
            {
                var productPrice = await _context.ProductPrices
                    .AsNoTracking()
                    .FirstOrDefaultAsync(pp => pp.ProductID == p.ID
                    && pp.TGKT > order.ThoiGianTao && pp.TGBD <= order.ThoiGianTao);
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
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> InHoaDon(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                    .ThenInclude(i => i.Product)
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
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                    .ThenInclude(i => i.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            var model = new OrderViewModel
            {
                ID = order.ID,
                ThoiGianTao = order.ThoiGianTao,
                UserName = order.User.Ten,
                CustomerName = order.Customer.Ten
            };
            foreach (var item in order.DetailOrders)
            {
                var itemVM = new DetailOrder
                {
                    Gia = item.Gia,
                    SoLuong = item.SoLuong,
                    Product = item.Product
                };
                model.DetailOrders.Add(itemVM);
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Customer)
                .Include(o => o.DetailOrders)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.ID == id);
            try
            {
                foreach (var i in order.DetailOrders)
                {
                    var productUpdate = await _context.Products.FirstOrDefaultAsync(p => p.ID == i.ProductID);
                    productUpdate.SoLuong += i.SoLuong;
                    _context.Products.Update(productUpdate);
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Đơn hàng \"{order.ID}\" đã xóa";
                return RedirectToAction("Index");
            }
            catch
            {
                var model = new OrderViewModel
                {
                    ID = order.ID,
                    ThoiGianTao = order.ThoiGianTao,
                    UserName = order.User.Ten,
                    CustomerName = order.Customer.Ten
                };
                foreach (var item in order.DetailOrders)
                {
                    var itemVM = new DetailOrder
                    {
                        Gia = item.Gia,
                        SoLuong = item.SoLuong,
                        Product = item.Product
                    };
                    model.DetailOrders.Add(itemVM);
                }
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình xóa đơn hàng, vui lòng thử lại vào thời gian khác!");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Thu ngân")]
        public IActionResult LoadCart()
        {
            var order = SessionHelper.GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            return PartialView("_CartAjax", order);
        }

        [HttpGet]
        [Authorize(Roles = "Thu ngân")]
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
            var order = SessionHelper
                .GetObjectFormJson<OrderCreateViewModel>(HttpContext.Session, "order");
            int index = isExist(id);
            if (index != -1)
            {
                if (order.DetailOrders[index].SoLuongBan < product.SoLuong)
                {
                    order.DetailOrders[index].SoLuongBan++;
                }
            }
            else
            {
                if (product.SoLuong > 0)
                {
                    var productPrice = _context.ProductPrices
                        .AsNoTracking()
                        .FirstOrDefault(pp => pp.ProductID == id
                            && pp.TGKT > order.ThoiGianTao && pp.TGBD <= order.ThoiGianTao);
                    var item = new ItemCreateViewModel
                    {
                        ProductID = product.ID,
                        ProductTen = product.Ten,
                        Gia = productPrice.Gia,
                        SoLuongBan = 1
                    };
                    order.DetailOrders.Add(item);
                }
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "order", order);
            return PartialView("_CartAjax", order);
        }

        [HttpGet]
        [Authorize(Roles = "Thu ngân")]
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
        [Authorize(Roles = "Thu ngân")]
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