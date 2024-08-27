using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.Controllers
{
    public class DanhMucThietBiController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public DanhMucThietBiController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        // GET: DanhMucThietBi
        public async Task<IActionResult> Index()
        {
            return View(await _context.DanhMucThietBi.ToListAsync());
        }

        // GET: DanhMucThietBi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucThietBi = await _context.DanhMucThietBi
                .FirstOrDefaultAsync(m => m.maDanhMuc == id);
            if (danhMucThietBi == null)
            {
                return NotFound();
            }

            return View(danhMucThietBi);
        }

        // GET: DanhMucThietBi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DanhMucThietBi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("maDanhMuc,tenDanhMuc")] DanhMucThietBi danhMucThietBi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMucThietBi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhMucThietBi);
        }

        // GET: DanhMucThietBi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucThietBi = await _context.DanhMucThietBi.FindAsync(id);
            if (danhMucThietBi == null)
            {
                return NotFound();
            }
            return View(danhMucThietBi);
        }

        // POST: DanhMucThietBi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("maDanhMuc,tenDanhMuc")] DanhMucThietBi danhMucThietBi)
        {
            if (id != danhMucThietBi.maDanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMucThietBi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucThietBiExists(danhMucThietBi.maDanhMuc))
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
            return View(danhMucThietBi);
        }

        // GET: DanhMucThietBi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMucThietBi = await _context.DanhMucThietBi
                .FirstOrDefaultAsync(m => m.maDanhMuc == id);
            if (danhMucThietBi == null)
            {
                return NotFound();
            }

            return View(danhMucThietBi);
        }

        // POST: DanhMucThietBi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMucThietBi = await _context.DanhMucThietBi.FindAsync(id);
            _context.DanhMucThietBi.Remove(danhMucThietBi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucThietBiExists(int id)
        {
            return _context.DanhMucThietBi.Any(e => e.maDanhMuc == id);
        }
    }
}
