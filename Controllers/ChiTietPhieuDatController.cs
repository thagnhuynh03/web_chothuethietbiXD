using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.Models;
using WebChoThueThietBiXD.ViewModels;

namespace WebChoThueThietBiXD.Controllers
{
    public class ChiTietPhieuDatController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public ChiTietPhieuDatController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        // GET: ChiTietPhieuDat
        public async Task<IActionResult> Index()
        {
            var webChoThueThietBiXDContext = _context.ChiTietPhieuDat
                .Include(c => c.PhieuDat)
                .ThenInclude(pd => pd.KhachHang)
                .Include(c => c.ThietBi);
            return View(await webChoThueThietBiXDContext.ToListAsync());
        }

        // GET: ChiTietPhieuDat/Create
        public IActionResult Create()
        {
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChiTietPhieuDat chiTietPhieuDat)
        {
            if (ModelState.IsValid)
            {
                chiTietPhieuDat.maPhieuDat = (int)HttpContext.Session.GetInt32("maPhieuDat");
                _context.Add(chiTietPhieuDat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "PhieuDat", new { id = chiTietPhieuDat.maPhieuDat });
            }
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", chiTietPhieuDat.maThietBi);

            return View(chiTietPhieuDat);
        }

        // GET: ChiTietPhieuDat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietPhieuDat = await _context.ChiTietPhieuDat
                .Include(c => c.ThietBi)
                .FirstOrDefaultAsync(c => c.maChiTietPhieuDat == id);
            if (chiTietPhieuDat == null)
            {
                return NotFound();
            }
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", chiTietPhieuDat.maThietBi);
            return View(chiTietPhieuDat);
        }

        // POST: ChiTietPhieuDat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChiTietPhieuDat chiTietPhieuDat)
        {
            if (id != chiTietPhieuDat.maChiTietPhieuDat)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietPhieuDat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietPhieuDatExists(chiTietPhieuDat.maChiTietPhieuDat))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit", "PhieuDat", new { id = chiTietPhieuDat.maPhieuDat });
            }
            // Truy vấn dữ liệu để lấy tên khách hàng
            var phieuDatList = _context.PhieuDat
                .Include(pd => pd.KhachHang) // Bao gồm thông tin khách hàng
                .Select(pd => new {
                    maPhieuDat = pd.maPhieuDat,
                    tenKhachHang = pd.KhachHang.tenKhachHang
                }).ToList();
            ViewData["maPhieuDat"] = new SelectList(phieuDatList, "maPhieuDat", "tenKhachHang", chiTietPhieuDat.maPhieuDat);
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", chiTietPhieuDat.maThietBi);
            return View(chiTietPhieuDat);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var chiTietPhieuDat = await _context.ChiTietPhieuDat.FindAsync(id);
            _context.ChiTietPhieuDat.Remove(chiTietPhieuDat);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "PhieuDat", new { id = chiTietPhieuDat.maPhieuDat });
        }

        private bool ChiTietPhieuDatExists(int id)
        {
            return _context.ChiTietPhieuDat.Any(e => e.maChiTietPhieuDat == id);
        }
    }
}
