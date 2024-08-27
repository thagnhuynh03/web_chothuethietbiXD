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
    public class ThietBiController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public ThietBiController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        // GET: ThietBi

        public async Task<IActionResult> Index()
        {
            ViewData["Controller"] = "ThietBi";
            var thietbi = await _context.ThietBi.Include(t => t.DanhMucThietBi).Include(t => t.Hang).ToListAsync();
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc", thietbi.FirstOrDefault().maDanhMuc);
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang", thietbi.FirstOrDefault().maHang);
            return View(thietbi);
        }

        // GET: ThietBi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thietBi = await _context.ThietBi
                .Include(t => t.DanhMucThietBi)
                .Include(t => t.Hang)
                .FirstOrDefaultAsync(m => m.maThietBi == id);
            if (thietBi == null)
            {
                return NotFound();
            }

            return View(thietBi);
        }

        // GET: ThietBi/Create
        public IActionResult Create()
        {
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc");
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang");
            return View();
        }

        // POST: ThietBi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("maThietBi,tenThietBi,moTa,giaThue,soLuongCon,maDanhMuc,maHang")] ThietBi thietBi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thietBi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc", thietBi.maDanhMuc);
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang", thietBi.maHang);
            return View(thietBi);
        }

        // GET: ThietBi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thietBi = await _context.ThietBi.FindAsync(id);
            if (thietBi == null)
            {
                return NotFound();
            }
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc", thietBi.maDanhMuc);
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang", thietBi.maHang);
            return View(thietBi);
        }

        // POST: ThietBi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("maThietBi,tenThietBi,moTa,giaThue,soLuongCon,maDanhMuc,maHang")] ThietBi thietBi)
        {
            if (id != thietBi.maThietBi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thietBi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThietBiExists(thietBi.maThietBi))
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
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc", thietBi.maDanhMuc);
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang", thietBi.maHang);
            return View(thietBi);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var thietbi = await _context.ThietBi.FindAsync(id);
            _context.ThietBi.Remove(thietbi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string query)
        {
            ViewData["Controller"] = "ThietBi";
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc");
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang");
            int queryAsInt;
            bool isIntQuery = int.TryParse(query, out queryAsInt);

            // Bắt đầu với truy vấn cơ bản
            var thietbiQuery = _context.ThietBi
                .Include(tb => tb.DanhMucThietBi)
                .Include(tb => tb.Hang)
                .AsQueryable();

            if (isIntQuery)
            {
                thietbiQuery = thietbiQuery.Where(tb => tb.maThietBi == queryAsInt || tb.giaThue == queryAsInt);
            }
            else
            {
                thietbiQuery = thietbiQuery.Where(tb => tb.tenThietBi.Contains(query)
                                                  || tb.Hang.tenHang.Contains(query)
                                                  || tb.moTa.Contains(query)
                                                  || tb.DanhMucThietBi.tenDanhMuc.Contains(query));
            }


            var thietbi = await thietbiQuery.ToListAsync();
            return View("Index", thietbi);
        }
        public async Task<IActionResult> Filter(int? maDanhMuc, int? maHang)
        {
            ViewData["Controller"] = "ThietBi";
            var thietbi = await _context.ThietBi
                .Include(tb => tb.DanhMucThietBi)
                .Include(tb => tb.Hang)
                .Where(tb => tb.maDanhMuc.Equals(maDanhMuc) || tb.maHang.Equals(maHang))
                .ToListAsync();
            ViewData["maDanhMuc"] = new SelectList(_context.DanhMucThietBi, "maDanhMuc", "tenDanhMuc", thietbi.FirstOrDefault().maDanhMuc);
            ViewData["maHang"] = new SelectList(_context.Hang, "maHang", "tenHang", thietbi.FirstOrDefault().maHang);
            return View("Index", thietbi);
        }

        private bool ThietBiExists(int id)
        {
            return _context.ThietBi.Any(e => e.maThietBi == id);
        }
    }
}
