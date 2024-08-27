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
    public class GioHangController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public GioHangController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }



        [HttpPost]
        public IActionResult Delete1(int maThietBi)
        {
            var userName = User.Identity.Name;
            var user = _context.TaiKhoan.FirstOrDefault(u => u.tenDangNhap == userName);

            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var cartItem = _context.GioHang.FirstOrDefault(c => c.maThietBi == maThietBi && c.maTaiKhoan == user.maTaiKhoan);

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Item not found in cart" });
            }

            _context.GioHang.Remove(cartItem);
            _context.SaveChanges();

            var cartCount = _context.GioHang.Count(c => c.maTaiKhoan == user.maTaiKhoan);

            return Json(new { success = true, cartCount = cartCount });
        }
        // GET: GioHang
        public async Task<IActionResult> Index()
        {
            var webChoThueThietBiXDContext = _context.GioHang.Include(g => g.TaiKhoan).Include(g => g.ThietBi);
            return View(await webChoThueThietBiXDContext.ToListAsync());
        }

        // GET: GioHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang
                .Include(g => g.TaiKhoan)
                .Include(g => g.ThietBi)
                .FirstOrDefaultAsync(m => m.maThietBi == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // GET: GioHang/Create
        public IActionResult Create()
        {
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap");
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");
            return View();
        }

        // POST: GioHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("maThietBi,maTaiKhoan")] GioHang gioHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap", gioHang.maTaiKhoan);
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", gioHang.maThietBi);
            return View(gioHang);
        }

        // GET: GioHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap", gioHang.maTaiKhoan);
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", gioHang.maThietBi);
            return View(gioHang);
        }

        // POST: GioHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("maThietBi,maTaiKhoan")] GioHang gioHang)
        {
            if (id != gioHang.maThietBi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangExists(gioHang.maThietBi))
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
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap", gioHang.maTaiKhoan);
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", gioHang.maThietBi);
            return View(gioHang);
        }

        // GET: GioHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang
                .Include(g => g.TaiKhoan)
                .Include(g => g.ThietBi)
                .FirstOrDefaultAsync(m => m.maThietBi == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int maThietBi)
        {
            var userName = User.Identity.Name;
            var user = _context.TaiKhoan.FirstOrDefault(u => u.tenDangNhap == userName);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var gioHang = await _context.GioHang
                .FirstOrDefaultAsync(g => g.maThietBi == maThietBi && g.maTaiKhoan == user.maTaiKhoan);

            if (gioHang != null)
            {
                _context.GioHang.Remove(gioHang);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        private bool GioHangExists(int id)
        {
            return _context.GioHang.Any(e => e.maThietBi == id);
        }
    }
}
