using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHangCore.Controllers
{
    public class CustomersController : Controller
    {
        private readonly QuanLyBanHangCoreContext _context;

        public CustomersController(QuanLyBanHangCoreContext context)
        {
            _context = context;
        }

        // GET: Customers
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.AsNoTracking().ToListAsync());
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Quản trị, Thu ngân, Thủ kho, Kế toán")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Thu ngân")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> Create([Bind("ID,Ten,SDT")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Khách hàng \"{customer.Ten}\" đã được thêm.";
                return RedirectToAction("Details", "Customers", new { id = customer.ID });
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,SDT")] Customer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"Khách hàng \"{customer.Ten}\" đã được cập nhật.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Customers", new { id = customer.ID });
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Thu ngân")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (!_context.Orders.Any(o => o.CustomerID == id))
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"Khách hàng \"{customer.Ten}\" đã được xóa.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Vì có đơn hàng của khách hàng này nên không thể xóa, chỉ có thể xóa khi không có đơn hàng của khách hàng này!");
            return View(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.ID == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> KiemTraSDT(string sdt, int id)
        {
            if (sdt == null)
            {
                return Json(true);
            }
            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.SDT.Equals(sdt) && c.ID != id);
            if (customer == null)
            {
                return Json(true);
            }
            return Json($"Số điện thoại \"{sdt}\" đã được sử dụng!");
        }
    }
}