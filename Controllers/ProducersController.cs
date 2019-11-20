using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class ProducersController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;

        public ProducersController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        // GET: Producers
        [Authorize(Roles = "Quản trị,Bán hàng,Thủ kho,Kế toán")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producers.AsNoTracking().ToListAsync());
        }

        // GET: Producers/Details/5
        [Authorize(Roles = "Quản trị,Bán hàng,Thủ kho,Kế toán")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // GET: Producers/Create
        [Authorize(Roles = "Thủ kho")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Create([Bind("ID,Ten")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Nhà sản xuất \"{producer.Ten}\" đã được thêm.";
                return RedirectToAction("Details", "Producers", new { id = producer.ID });
            }
            return View(producer);
        }

        // GET: Producers/Edit/5
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers.FindAsync(id);
            if (producer == null)
            {
                return NotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten")] Producer producer)
        {
            if (id != producer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producer);
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"Nhà sản xuất \"{producer.Ten}\" đã được cập nhật.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducerExists(producer.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Producers", new { id = producer.ID });
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thủ kho")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producer = await _context.Producers.FindAsync(id);
            if (!_context.Products.Any(p => p.ProducerID == id))
            {
                _context.Producers.Remove(producer);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Nhà sản xuất \"{producer.Ten}\" đã được xóa.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Vì có sản phẩm thuộc nhà sản xuất này nên không thể xóa nó, chỉ có thể xóa nó khi không có sản phẩm thuộc nó!");
            return View(producer);
        }

        private bool ProducerExists(int id)
        {
            return _context.Producers.Any(e => e.ID == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsProducerNameExists(string ten, int id)
        {
            var producer = await _context.Producers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Ten == ten && p.ID != id);
            if (producer == null)
            {
                return Json(true);
            }
            return Json($"Tên nhà sản xuất \"{ten}\" đã được sử dụng!");
        }
    }
}