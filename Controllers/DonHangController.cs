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
    public class DonHangController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public DonHangController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        // GET: DonHang
        public async Task<IActionResult> Index()
        {
            ViewData["Controller"] = "DonHang";
            var donHang = await _context.DonHang
                .Include(d => d.NhanVien)
                .Include(d => d.PhieuDat)
                .ThenInclude(pd => pd.KhachHang)
                .Select(donHang => new DonHang
                {
                    maDonHang = donHang.maDonHang,
                    trangThaiDonHang = donHang.trangThaiDonHang,
                    ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                    ngayThanhToan = donHang.ngayThanhToan ?? null,
                    maPhieuDat = donHang.maPhieuDat,
                    PhieuDat = donHang.PhieuDat,
                    maNhanVien = donHang.maNhanVien,
                    NhanVien = donHang.NhanVien
                })
                .ToListAsync();
            return View(donHang);
        }

        // GET: DonHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Controller"] = "DonHang";
            if (id == null)
            {
                return NotFound();
            }
            var donHang = await _context.DonHang
                .Include(d => d.NhanVien)
                .Include(d => d.PhieuDat)
                .ThenInclude(pd => pd.KhachHang)
                .Include(d => d.PhieuDat)
                .ThenInclude(pd => pd.ChiTietPhieuDats)
                .ThenInclude(ct => ct.ThietBi)
                .Select(donHang => new DonHang
                {
                    maDonHang = donHang.maDonHang,
                    trangThaiDonHang = donHang.trangThaiDonHang,
                    ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                    ngayThanhToan = donHang.ngayThanhToan ?? null,
                    maPhieuDat = donHang.maPhieuDat,
                    PhieuDat = donHang.PhieuDat,
                    maNhanVien = donHang.maNhanVien,
                    NhanVien = donHang.NhanVien
                })
                .FirstOrDefaultAsync(dh => dh.maDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }
            ViewBag.TongTien = TongTien(donHang.PhieuDat.ChiTietPhieuDats);
            return View(donHang);
        }

        // GET: DonHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHang
                .Include(d => d.NhanVien)
                .Include(d => d.PhieuDat)
                .ThenInclude(pd => pd.KhachHang)
                .Select(donHang => new DonHang
                {
                    maDonHang = donHang.maDonHang,
                    trangThaiDonHang = donHang.trangThaiDonHang,
                    ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                    ngayThanhToan = donHang.ngayThanhToan ?? null,
                    maPhieuDat = donHang.maPhieuDat,
                    PhieuDat = donHang.PhieuDat,
                    maNhanVien = donHang.maNhanVien,
                    NhanVien = donHang.NhanVien
                })
                .FirstOrDefaultAsync(dh => dh.maDonHang == id);
            return View(donHang);
        }

        // POST: DonHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DonHang donHang)
        {
            if (id != donHang.maDonHang)
            {
                return NotFound();
            }

            if (donHang.trangThaiDonHang != null || donHang.ngayThanhToan == null || donHang.ngayGiaoHang == null)
            {
                try
                {
                    var DonHang = await _context.DonHang
                   .Include(d => d.NhanVien)
                   .Include(d => d.PhieuDat)
                   .ThenInclude(pd => pd.KhachHang)
                   .Select(donHang => new DonHang
                   {
                       maDonHang = donHang.maDonHang,
                       trangThaiDonHang = donHang.trangThaiDonHang,
                       ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                       ngayThanhToan = donHang.ngayThanhToan ?? null,
                       maPhieuDat = donHang.maPhieuDat,
                       PhieuDat = donHang.PhieuDat,
                       maNhanVien = donHang.maNhanVien,
                       NhanVien = donHang.NhanVien
                   })
                   .FirstOrDefaultAsync(dh => dh.maDonHang == id);
                    DonHang.trangThaiDonHang = donHang.trangThaiDonHang;
                    DonHang.ngayGiaoHang = donHang.ngayGiaoHang;
                    DonHang.ngayThanhToan = donHang.ngayThanhToan;
                    _context.Update(DonHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.maDonHang))
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
            return View(donHang);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DonHang = await _context.DonHang
                   .Include(d => d.NhanVien)
                   .Include(d => d.PhieuDat)
                   .ThenInclude(pd => pd.KhachHang)
                   .Select(donHang => new DonHang
                   {
                       maDonHang = donHang.maDonHang,
                       trangThaiDonHang = donHang.trangThaiDonHang,
                       ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                       ngayThanhToan = donHang.ngayThanhToan ?? null,
                       maPhieuDat = donHang.maPhieuDat,
                       PhieuDat = donHang.PhieuDat,
                       maNhanVien = donHang.maNhanVien,
                       NhanVien = donHang.NhanVien
                   })
                   .FirstOrDefaultAsync(dh => dh.maDonHang == id);
            _context.Remove(DonHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string query)
        {
            ViewData["Controller"] = "DonHang";
            int queryAsInt;
            bool isIntQuery = int.TryParse(query, out queryAsInt);

            // Bắt đầu với truy vấn cơ bản
            var donHangQuery = _context.DonHang
                .Include(d => d.NhanVien)
                .Include(d => d.PhieuDat)
                .ThenInclude(pd => pd.KhachHang)
                .Select(donHang => new DonHang
                {
                    maDonHang = donHang.maDonHang,
                    trangThaiDonHang = donHang.trangThaiDonHang,
                    ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                    ngayThanhToan = donHang.ngayThanhToan ?? null,
                    maPhieuDat = donHang.maPhieuDat,
                    PhieuDat = donHang.PhieuDat,
                    maNhanVien = donHang.maNhanVien,
                    NhanVien = donHang.NhanVien
                })
                .AsQueryable();

            if (isIntQuery)
            {
                donHangQuery = donHangQuery.Where(tb => tb.maDonHang == queryAsInt);
            }
            else
            {
                donHangQuery = donHangQuery.Where(tb => tb.PhieuDat.KhachHang.tenKhachHang.Contains(query)
                                                  || tb.trangThaiDonHang.Contains(query)
                                                  || tb.NhanVien.tenNhanVien.Contains(query));
            }


            var donHang = await donHangQuery.ToListAsync();
            return View("Index", donHang);
        }

        public async Task<IActionResult> Filter(string? trangThai, DateTime? start_thanhToan, DateTime? end_thanhToan, DateTime? start_giaoHang, DateTime? end_giaoHang)
        {
            ViewData["Controller"] = "DonHang";
            var query = _context.DonHang
                       .Include(d => d.NhanVien)
                       .Include(d => d.PhieuDat)
                       .ThenInclude(pd => pd.KhachHang)
                       .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(donHang => donHang.trangThaiDonHang == trangThai);
            }

            if (start_thanhToan.HasValue && end_thanhToan.HasValue)
            {
                query = query.Where(donHang => donHang.ngayThanhToan.HasValue &&
                                               donHang.ngayThanhToan.Value >= start_thanhToan.Value &&
                                               donHang.ngayThanhToan.Value <= end_thanhToan.Value);
            }
            if (start_giaoHang.HasValue && end_giaoHang.HasValue)
            {
                query = query.Where(donHang => donHang.ngayGiaoHang.HasValue &&
                                               donHang.ngayGiaoHang.Value >= start_giaoHang.Value &&
                                               donHang.ngayGiaoHang.Value <= end_giaoHang.Value);
            }

            var donHang = await query
               .Select(donHang => new DonHang
               {
                   maDonHang = donHang.maDonHang,
                   trangThaiDonHang = donHang.trangThaiDonHang,
                   ngayGiaoHang = donHang.ngayGiaoHang ?? null,
                   ngayThanhToan = donHang.ngayThanhToan ?? null,
                   maPhieuDat = donHang.maPhieuDat,
                   PhieuDat = donHang.PhieuDat,
                   maNhanVien = donHang.maNhanVien,
                   NhanVien = donHang.NhanVien
               })
               .ToListAsync();

            return View("Index", donHang);
        }

        public decimal TongTien(ICollection<ChiTietPhieuDat> chiTietPhieuDats)
        {
            decimal tong = 0;
            foreach (var item in chiTietPhieuDats)
            {
                tong += item.giaThueNgay * item.soLuongThue * (item.ngayKetThucThue - item.ngayBatDauThue).Days;
            }
            return tong;
        }

        //Đọc trước khi xóa giùm ??
        public async Task<IActionResult> StaffIndex()
        {
            var webChoThueThietBiXDContext = _context.DonHang
                .Include(d => d.NhanVien)
                .Include(d => d.PhieuDat)
                .ThenInclude(pd => pd.KhachHang);  // Bao gồm thông tin khách hàng;
            return View(await webChoThueThietBiXDContext.ToListAsync());
        }
        //Tìm kiếm của nhân viên
        public async Task<IActionResult> DonHangSearchStaff(string query)
        {
            ViewData["Controller"] = "DonHang";
            int queryAsInt;
            bool isIntQuery = int.TryParse(query, out queryAsInt);

            var donHangQuery = _context.DonHang
                .Include(dh => dh.PhieuDat)
                    .ThenInclude(pd => pd.KhachHang)
                .Include(dh => dh.NhanVien)
                .AsQueryable();

            if (isIntQuery)
            {
                donHangQuery = donHangQuery.Where(dh => dh.maDonHang == queryAsInt
                                                     || dh.PhieuDat.soDienThoaiGiaoHang.Equals(query));
            }
            else
            {
                donHangQuery = donHangQuery.Where(dh => dh.PhieuDat.KhachHang.tenKhachHang.Contains(query)
                                                     || dh.PhieuDat.soDienThoaiGiaoHang.Contains(query)
                                                     || dh.PhieuDat.diaCHiGiaoHang.Contains(query));
            }

            var donHang = await donHangQuery.ToListAsync();
            return View("StaffIndex", donHang);
        }
        public async Task<IActionResult> DonHangFilterStaff(string trangThai)
        {
            ViewData["Controller"] = "DonHang";

            var donHangQuery = _context.DonHang
                .Include(dh => dh.PhieuDat)
                    .ThenInclude(pd => pd.KhachHang)
                .Include(dh => dh.NhanVien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
            {
                donHangQuery = donHangQuery.Where(dh => dh.trangThaiDonHang.Equals(trangThai));
            }

            var donHang = await donHangQuery.ToListAsync();
            return View("StaffIndex", donHang);
        }
        
        private bool DonHangExists(int id)
        {
            return _context.DonHang.Any(e => e.maDonHang == id);
        }
    }
}
