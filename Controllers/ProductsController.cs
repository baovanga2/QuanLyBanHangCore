using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using QuanLyBanHangCore.Models.ViewModels;

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
        public async Task<IActionResult> Index()
        {
            DateTime dateTimeNow = DateTime.Now;
            var products = await _context.Products.Include(p => p.Category).Include(p => p.Producer).ToListAsync();
            List<ProductWithCurrentPrice> productWithCurrentPrices = new List<ProductWithCurrentPrice>(); 
            foreach (Product p in products)
            {
                var productPrice = _context.ProductPrices.FirstOrDefault(pp => pp.ProductID == p.ID && pp.TGKT > dateTimeNow);
                productWithCurrentPrices.Add(new ProductWithCurrentPrice { Product = p , Gia = productPrice.Gia});
            }
            return View(productWithCurrentPrices);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
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
            DateTime dateTimeNow = DateTime.Now;
            List<ProductPrice> productPrices = _context.ProductPrices.Where(pp => pp.ProductID == id).OrderByDescending(pp => pp.TGKT).ToList();

            ProductPrice productPrice = productPrices.First(pp => pp.TGKT > dateTimeNow);

            ProductWithPriceList productWithPriceList = new ProductWithPriceList { Product = product, Gia = productPrice.Gia,ProductPrices = productPrices };

            return View(productWithPriceList);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten");
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product, Gia")] ProductWithCurrentPrice productWithCurrentPrice)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTimeNow = DateTime.Now;
                _context.Add(productWithCurrentPrice.Product);
                _context.SaveChanges();
                ProductPrice productPrice = new ProductPrice
                {
                    Gia = productWithCurrentPrice.Gia,
                    TGBD = dateTimeNow,
                    TGKT = DateTime.Parse("9999-1-1"),
                    ProductID = productWithCurrentPrice.Product.ID
                };
                _context.Add(productPrice);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"{productWithCurrentPrice.Product.Ten} đã được thêm";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten", productWithCurrentPrice.Product.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten", productWithCurrentPrice.Product.ProducerID);
            return View(productWithCurrentPrice);
        }

        // GET: Products/Edit/5
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
            var productPrice = _context.ProductPrices.First(pp => pp.ProductID == id && pp.TGKT > dateTimeNow);
            ProductWithCurrentPrice productWithCurrentPrice = new ProductWithCurrentPrice
            {
                Product = product,
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
        public async Task<IActionResult> Edit(int id, [Bind("Product, Gia")] ProductWithCurrentPrice productWithCurrentPrice)
        {
            if (id != productWithCurrentPrice.Product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dateTimeNow = DateTime.Now;
                    _context.Update(productWithCurrentPrice.Product);
                    ProductPrice productPrice = _context.ProductPrices.First(pp => pp.ProductID == id && pp.TGKT > dateTimeNow);
                    if (productWithCurrentPrice.Gia != productPrice.Gia)
                    {
                        ProductPrice productPriceNew = new ProductPrice
                        {
                            Gia = productWithCurrentPrice.Gia,
                            TGBD = dateTimeNow,
                            TGKT = DateTime.Parse("9999-1-1"),
                            ProductID = productWithCurrentPrice.Product.ID
                        };
                        _context.Add(productPriceNew);
                        productPrice.TGKT = dateTimeNow;
                        _context.Update(productPrice);
                        await _context.SaveChangesAsync();
                        TempData["messageSuccess"] = $"{productWithCurrentPrice.Product.Ten} đã được cập nhật";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productWithCurrentPrice.Product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Ten", productWithCurrentPrice.Product.CategoryID);
            ViewData["ProducerID"] = new SelectList(_context.Producers, "ID", "Ten", productWithCurrentPrice.Product.ProducerID);
            return View(productWithCurrentPrice);
        }

        // GET: Products/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
