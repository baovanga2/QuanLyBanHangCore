using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;

        public CategoriesController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.AsNoTracking().ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Loại sản phẩm \"{category.Ten}\" đã được thêm.";
                return RedirectToAction("Details", "Categories", new { id = category.ID });
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten")] Category category)
        {
            if (id != category.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"Loại sản phẩm \"{category.Ten}\" đã được cập nhật.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Categories", new { id = category.ID });
            }
            return View(category);
        }
        
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (!_context.Products.Any(p => p.CategoryID == id))
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Loại sản phẩm \"{category.Ten}\" đã được xóa.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Vì có sản phẩm thuộc loại sản phẩm này nên không thể xóa nó, chỉ có thể xóa nó khi không có sản phẩm thuộc nó!");
            return View(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
                
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsCategoryNameExists(string ten, int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Ten == ten && c.ID != id);
            if (category == null)
            {
                return Json(true);
            }
            return Json($"Tên \"{ten}\" đã được sử dụng!");
        }
    }
}