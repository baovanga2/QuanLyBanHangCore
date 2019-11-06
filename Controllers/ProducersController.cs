﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHangCore.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producers.AsNoTracking().ToListAsync());
        }

        // GET: Producers/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                if (ProducerExists(0 ,producer.Ten))
                {
                    ModelState.AddModelError("Ten", "Tên đã được sử dụng.");
                    return View(producer);
                }
                _context.Add(producer);
                await _context.SaveChangesAsync();
                TempData["messageSuccess"] = $"\"{producer.Ten}\" đã được thêm.";
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        // GET: Producers/Edit/5
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
                    if (ProducerExists(id, producer.Ten))
                    {
                        ModelState.AddModelError("Ten", "Tên đã được sử dụng.");
                        return View(producer);
                    }
                    _context.Update(producer);
                    await _context.SaveChangesAsync();
                    TempData["messageSuccess"] = $"\"{producer.Ten}\" đã được cập nhật.";
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
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producer = await _context.Producers.FindAsync(id);
            _context.Producers.Remove(producer);
            await _context.SaveChangesAsync();
            TempData["messageSuccess"] = $"\"{producer.Ten}\" đã được xóa.";
            return RedirectToAction(nameof(Index));
        }

        private bool ProducerExists(int id)
        {
            return _context.Producers.Any(e => e.ID == id);
        }

        private bool ProducerExists(int id, string ten)
        {
            return _context.Producers.Any(p => p.Ten == ten && p.ID != id);
        }
    }
}
