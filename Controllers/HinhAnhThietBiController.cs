using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.Models;
using WebChoThueThietBiXD.ViewModels;

namespace WebChoThueThietBiXD.Controllers
{
    public class HinhAnhThietBiController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HinhAnhThietBiController(WebChoThueThietBiXDContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: HinhAnhThietBi
        public async Task<IActionResult> Index()
        {
            var webChoThueThietBiXDContext = _context.HinhAnhThietBi.Include(h => h.ThietBi);
            return View(await webChoThueThietBiXDContext.ToListAsync());
        }

        // GET: HinhAnhThietBi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hinhAnhThietBi = await _context.HinhAnhThietBi
                .Include(h => h.ThietBi)
                .FirstOrDefaultAsync(m => m.maHinhAnh == id);
            if (hinhAnhThietBi == null)
            {
                return NotFound();
            }

            return View(hinhAnhThietBi);
        }

        // GET: HinhAnhThietBi/Create
        public IActionResult Create()
        {
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");
            return View();
        }

        // POST: HinhAnhThietBi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HinhAnhViewModel hinhAnhThietBi)
        {
            if (ModelState.IsValid)
            {   
                // Lưu hình ảnh vào đường dẫn cố định
                if (hinhAnhThietBi.hinhAnh != null && hinhAnhThietBi.hinhAnh.Length > 0)
                {
                    var fileName = Path.GetFileName(hinhAnhThietBi.hinhAnh.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img-product", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhThietBi.hinhAnh.CopyToAsync(stream);
                    }
                    HinhAnhThietBi hinhAnh = new HinhAnhThietBi()
                    {
                        hinhAnh = fileName,
                        maThietBi = hinhAnhThietBi.maThietBi
                    };
                }
                
                _context.Add(hinhAnhThietBi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", hinhAnhThietBi.maThietBi);
            return View(hinhAnhThietBi);
        }

        // GET: HinhAnhThietBi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hinhAnhThietBi = await _context.HinhAnhThietBi.FindAsync(id);
            if (hinhAnhThietBi == null)
            {
                return NotFound();
            }
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", hinhAnhThietBi.maThietBi);
            return View(hinhAnhThietBi);
        }

        // POST: HinhAnhThietBi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("maHinhAnh,hinhAnh,maThietBi")] HinhAnhThietBi hinhAnhThietBi, IFormFile file)
        {
            if (id != hinhAnhThietBi.maHinhAnh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    // Lưu hình ảnh vào đường dẫn cố định
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img-product", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        hinhAnhThietBi.hinhAnh = "/img-product/" + fileName; // Cập nhật đường dẫn hình ảnh trong sản phẩm
                    }
                    _context.Update(hinhAnhThietBi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HinhAnhThietBiExists(hinhAnhThietBi.maHinhAnh))
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
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", hinhAnhThietBi.maThietBi);
            return View(hinhAnhThietBi);
        }

        // GET: HinhAnhThietBi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hinhAnhThietBi = await _context.HinhAnhThietBi
                .Include(h => h.ThietBi)
                .FirstOrDefaultAsync(m => m.maHinhAnh == id);
            if (hinhAnhThietBi == null)
            {
                return NotFound();
            }

            return View(hinhAnhThietBi);
        }

        // POST: HinhAnhThietBi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hinhAnhThietBi = await _context.HinhAnhThietBi.FindAsync(id);
            _context.HinhAnhThietBi.Remove(hinhAnhThietBi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HinhAnhThietBiExists(int id)
        {
            return _context.HinhAnhThietBi.Any(e => e.maHinhAnh == id);
        }
    }
}
