using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;

        public ProductsController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles = "Quản trị,Bán hàng,Thủ kho,Kế toán")]
        public async Task<IActionResult> Index()
        {
            DateTime dateTimeNow = DateTime.Now;
            var products = await _context.Products.Include(p => p.Category).Include(p => p.Producer).AsNoTracking().ToListAsync();
            List<ProductWithCurrentPrice> productWithCurrentPrices = new List<ProductWithCurrentPrice>();
            foreach (Product p in products)
            {
                var productPrice = await _context.ProductPrices.AsNoTracking().FirstOrDefaultAsync(pp => pp.ProductID == p.ID && pp.TGKT > dateTimeNow);
                var productWithCurrentPrice = new ProductWithCurrentPrice
                {
                    ID = p.ID,
                    Ten = p.Ten,
                    SoLuong = p.SoLuong,
                    ProducerID = p.ProducerID,
                    Producer = p.Producer,
                    CategoryID = p.CategoryID,
                    Category = p.Category,
                    Gia = productPrice.Gia
                };
                productWithCurrentPrices.Add(productWithCurrentPrice);
            }
            return View(productWithCurrentPrices);
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Quản trị,Bán hàng,Thủ kho,Kế toán")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            DateTime dateTimeNow = DateTime.Now;
            List<ProductPrice> productPrices = _context.ProductPrices.Where(pp => pp.ProductID == id).OrderByDescending(pp => pp.TGKT).ToList();

            ProductPrice productPrice = productPrices.First(pp => pp.TGKT > dateTimeNow);

            ProductWithPriceList productWithPriceList = new ProductWithPriceList { Product = product, Gia = productPrice.Gia, ProductPrices = productPrices };

            return View(productWithPriceList);
        }

        // GET: Products/Create
        [Authorize(Roles = "Thủ kho")]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten");
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Ten, SoLuong, ProducerID, CategoryID, Gia")] ProductWithCurrentPrice productWithCurrentPrice)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTimeNow = DateTime.Now;
                var product = new Product
                {
                    ID = productWithCurrentPrice.ID,
                    Ten = productWithCurrentPrice.Ten,
                    SoLuong = productWithCurrentPrice.SoLuong,
                    ProducerID = productWithCurrentPrice.ProducerID,
                    CategoryID = productWithCurrentPrice.CategoryID
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                var productPrice = new ProductPrice
                {
                    Gia = productWithCurrentPrice.Gia,
                    TGBD = dateTimeNow,
                    TGKT = DateTime.Parse("9999-1-1"),
                    ProductID = product.ID
                };
                _context.Add(productPrice);
                _context.SaveChanges();
                TempData["messageSuccess"] = $"Sản phẩm \"{productWithCurrentPrice.Ten}\" đã được thêm";
                return RedirectToAction("Details", "Products", new { id = product.ID });
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten", productWithCurrentPrice.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten", productWithCurrentPrice.ProducerID);
            return View(productWithCurrentPrice);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dateTimeNow = DateTime.Now;
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productPrice = await _context.ProductPrices.FirstAsync(pp => pp.ProductID == id && pp.TGKT > dateTimeNow);
            ProductWithCurrentPrice productWithCurrentPrice = new ProductWithCurrentPrice
            {
                ID = product.ID,
                Ten = product.Ten,
                SoLuong = product.SoLuong,
                Gia = productPrice.Gia
            };

            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten", product.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten", product.ProducerID);
            return View(productWithCurrentPrice);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Ten, SoLuong, ProducerID, CategoryID, Gia")] ProductWithCurrentPrice productWithCurrentPrice)
        {
            if (id != productWithCurrentPrice.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dateTimeNow = DateTime.Now;
                    var product = new Product
                    {
                        ID = productWithCurrentPrice.ID,
                        Ten = productWithCurrentPrice.Ten,
                        SoLuong = productWithCurrentPrice.SoLuong,
                        ProducerID = productWithCurrentPrice.ProducerID,
                        CategoryID = productWithCurrentPrice.CategoryID
                    };
                    _context.Update(product);
                    ProductPrice productPrice = _context.ProductPrices.First(pp => pp.ProductID == id && pp.TGKT > dateTimeNow);
                    if (productWithCurrentPrice.Gia != productPrice.Gia)
                    {
                        ProductPrice productPriceNew = new ProductPrice
                        {
                            Gia = productWithCurrentPrice.Gia,
                            TGBD = dateTimeNow,
                            TGKT = DateTime.Parse("9999-1-1"),
                            ProductID = productWithCurrentPrice.ID
                        };
                        _context.Add(productPriceNew);
                        productPrice.TGKT = dateTimeNow;
                        _context.Update(productPrice);
                    }
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"Sản phẩm \"{productWithCurrentPrice.Ten}\" đã được cập nhật";
                    return RedirectToAction("Details", "Products", new { id = productWithCurrentPrice.ID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productWithCurrentPrice.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten", productWithCurrentPrice.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten", productWithCurrentPrice.ProducerID);
            return View(productWithCurrentPrice);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = await _context.Products
                .Include(p => p.ProductPrices)
                .SingleAsync(p => p.ID == id);
            if (!_context.DetailOrders.Any(d => d.ProductID == id))
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Sản phẩm \"{product.Ten}\" đã được xóa";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Vì có đơn hàng có sản phẩm này nên không thể xóa, chỉ có thể xóa khi không có đơn hàng có sản phẩm này!");
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProductNameExists(string ten, int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Ten == ten && p.ID != id);
            if (product == null)
            {
                return Json(true);
            }
            return Json($"Tên sản phẩm \"{ten}\" đã được sử dụng!");
        }
    }
}