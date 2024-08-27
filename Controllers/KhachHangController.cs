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
    public class KhachHangController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public KhachHangController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        public IActionResult Profile(int id)
        {
            var khachHang = _context.KhachHang.FirstOrDefault(k => k.maTaiKhoan == id);

            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        [Route("UpdateProfile")]
        public IActionResult UpdateProfile(KhachHang model, string currentPassword, string newPassword, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                var khachHang = _context.KhachHang.FirstOrDefault(k => k.maTaiKhoan == model.maTaiKhoan);
                if (khachHang != null)
                {
                    khachHang.tenKhachHang = model.tenKhachHang;
                    khachHang.Email = model.Email;
                    khachHang.soDienThoai = model.soDienThoai;
                    khachHang.diaChi = model.diaChi;

                    if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword))
                    {
                        if (newPassword != confirmPassword)
                        {
                            ModelState.AddModelError(string.Empty, "Mật khẩu mới và xác nhận mật khẩu không khớp.");
                            return View("Profile", model);
                        }

                        var taiKhoan = _context.TaiKhoan.FirstOrDefault(t => t.maTaiKhoan == model.maTaiKhoan);
                        if (taiKhoan != null && taiKhoan.matKhau == currentPassword)
                        {
                            taiKhoan.matKhau = newPassword;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không đúng.");
                            return View("Profile", model);
                        }
                    }

                    try
                    {
                        _context.SaveChanges();
                        TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                        return RedirectToAction("Profile", new { id = model.maTaiKhoan });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi lưu thay đổi: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy khách hàng.");
                }
            }

            return View("Profile", model);
        }

        [HttpGet]
        public async Task<IActionResult> Orders(string state)
        {
            var userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Index");
            }

            var user = await _context.TaiKhoan
                .Include(u => u.khachHangs)
                .FirstOrDefaultAsync(u => u.tenDangNhap == userName);

            if (user == null || user.khachHangs == null || !user.khachHangs.Any())
            {
                return RedirectToAction("Index");
            }

            var khachHang = user.khachHangs.FirstOrDefault();
            if (khachHang == null)
            {
                return RedirectToAction("Index");
            }

            var ordersQuery = _context.PhieuDat
                .Include(pd => pd.ChiTietPhieuDats)
                .ThenInclude(ct => ct.ThietBi)
                .Include(pd => pd.DonHangs)
                .Where(pd => pd.maKhachHang == khachHang.maKhachHang);

            if (!string.IsNullOrEmpty(state))
            {

                switch (state.ToLower())
                {
                    case "orders":
                        ordersQuery = ordersQuery.Where(o => o.trangThaiPhieuDat == "Chưa xác nhận");
                        break;
                    case "all":
                        ordersQuery = ordersQuery.Where(o => o.trangThaiPhieuDat == "Chưa xác nhận" || o.trangThaiPhieuDat == "Đã xác nhận");
                        break;
                    case "processing":
                        ordersQuery = ordersQuery.Where(o => o.DonHangs.Any(dh => dh.trangThaiDonHang == "Đang xử lý"));
                        break;
                    case "delivering":
                        ordersQuery = ordersQuery.Where(o => o.DonHangs.Any(dh => dh.trangThaiDonHang == "Đang giao"));
                        break;
                    case "delivered":
                        ordersQuery = ordersQuery.Where(o => o.DonHangs.Any(dh => dh.trangThaiDonHang == "Đã giao"));
                        break;
                    case "cancelled":
                        ordersQuery = ordersQuery.Where(o => o.trangThaiPhieuDat == "Đã hủy");
                        break;
                    default:
                        ordersQuery = ordersQuery.Where(o => o.trangThaiPhieuDat == "Chưa xác nhận" || o.trangThaiPhieuDat == "Đã xác nhận");
                        break;
                }
            }

            var orders = await ordersQuery.ToListAsync();
            if (state != null)
            {
                ViewBag.state = state?.ToLower();
            }
            else
            {
                ViewBag.state = "all";
            }


            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.PhieuDat.FindAsync(id);
            if (order != null)
            {
                order.trangThaiPhieuDat = "Đã hủy";
                await _context.SaveChangesAsync();
            }
            ViewBag.state = "ALL";
            return RedirectToAction("Orders");
        }


        // GET: KhachHang
        public async Task<IActionResult> Index()
        {
            var webChoThueThietBiXDContext = _context.KhachHang.Include(k => k.TaiKhoan);
            return View(await webChoThueThietBiXDContext.ToListAsync());
        }

        // GET: KhachHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHang
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.maKhachHang == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: KhachHang/Create
        public IActionResult Create()
        {
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap");
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("maKhachHang,tenKhachHang,ngaySinh,Email,soDienThoai,diaChi,maTaiKhoan")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap", khachHang.maTaiKhoan);
            return View(khachHang);
        }

        // GET: KhachHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap", khachHang.maTaiKhoan);
            return View(khachHang);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("maKhachHang,tenKhachHang,ngaySinh,Email,soDienThoai,diaChi,maTaiKhoan")] KhachHang khachHang)
        {
            if (id != khachHang.maKhachHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.maKhachHang))
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
            ViewData["maTaiKhoan"] = new SelectList(_context.TaiKhoan, "maTaiKhoan", "tenDangNhap", khachHang.maTaiKhoan);
            return View(khachHang);
        }

        // GET: KhachHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHang
                .Include(k => k.TaiKhoan)
                .FirstOrDefaultAsync(m => m.maKhachHang == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            _context.KhachHang.Remove(khachHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHang.Any(e => e.maKhachHang == id);
        }
    }
}
