using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.Models;
using WebChoThueThietBiXD.ViewModels;

namespace WebChoThueThietBiXD.Controllers
{
    public class PhieuDatController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public PhieuDatController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        public IActionResult PhieuDat()
        {
            return View();
        }

       public IActionResult TaoPhieuDat(PhieuDatModel model, List<ChiTietPhieuDat> chiTietPhieuDats)
{
    if (ModelState.IsValid)
    {
        var userName = User.Identity.Name;
        var user = _context.TaiKhoan.Include(u => u.khachHangs).FirstOrDefault(u => u.tenDangNhap == userName);

        if (user == null)
        {
            return Json(new { success = false, message = "User not logged in" });
        }
        var makh = _context.KhachHang.Where(u => u.maTaiKhoan == user.maTaiKhoan).FirstOrDefault().maKhachHang;
        var phieuDat = new PhieuDat
        {
            ngayTaoPhieu = DateTime.Now,
            trangThaiPhieuDat = "Chưa xác nhận",
            diaCHiGiaoHang = model.diaCHiGiaoHang,
            soDienThoaiGiaoHang = model.soDienThoaiGiaoHang,
            ghiChu = model.ghiChu,
            maKhachHang = makh,
            ChiTietPhieuDats = model.chiTietPhieuDats
        };
        _context.PhieuDat.Add(phieuDat);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Đặt đơn thành công!";
        return RedirectToAction("PhieuDat");
    }

    var errors = ModelState.Values.SelectMany(v => v.Errors);
    foreach (var error in errors)
    {
        Console.WriteLine(error.ErrorMessage);
    }
    TempData["Error"] = "Đặt đơn không thành công!";
    return RedirectToAction("PhieuDat"); // Sửa đổi nếu cần
}

        // GET: PhieuDat
        public async Task<IActionResult> Index()
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDat = await _context.PhieuDat
                                .Include(p => p.KhachHang)
                                .Select(phieuDat => new PhieuDat
                                {
                                    maPhieuDat = phieuDat.maPhieuDat,
                                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                                    ghiChu = phieuDat.ghiChu ?? "No notes",
                                    KhachHang = phieuDat.KhachHang
                                })
                                .ToListAsync();
            return View(phieuDat);
        }

        // GET: PhieuDat
        public async Task<IActionResult> StaffIndex()
        {
            var webChoThueThietBiXDContext = await _context.PhieuDat
            .Include(p => p.KhachHang)
            .Select(phieuDat => new PhieuDat
            {
                maPhieuDat = phieuDat.maPhieuDat,
                ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                KhachHang = phieuDat.KhachHang
            })
            .ToListAsync();

            return View(webChoThueThietBiXDContext);
        }

        // GET: PhieuDat/Create
        public IActionResult Create()
        {
            ViewData["Controller"] = "PhieuDat";
            ViewData["maKhachHang"] = new SelectList(_context.KhachHang, "maKhachHang", "tenKhachHang");
            var chiTietPhieuDatsJson = HttpContext.Session.GetString("ChiTietPhieuDats");
            if (!string.IsNullOrEmpty(chiTietPhieuDatsJson))
            {
                List<ChiTietPhieuDat> chiTietPhieuDats = JsonConvert.DeserializeObject<List<ChiTietPhieuDat>>(chiTietPhieuDatsJson);
                var model = chiTietPhieuDats.Where(p => p.maPhieuDat == 0).ToList();
                return View(model);
            }
            else
            {
                var model = new PhieuDatModel()
                {
                    chiTietPhieuDats = new List<ChiTietPhieuDat>()
                };
                return View(model);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhieuDatModel model)
        {
            if (ModelState.IsValid)
            {
                var phieuDat = new PhieuDat
                {
                    ngayTaoPhieu = DateTime.Now,
                    trangThaiPhieuDat = "Chưa xác nhận",
                    diaCHiGiaoHang = model.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = model.soDienThoaiGiaoHang,
                    ghiChu = model.ghiChu,
                    maKhachHang = model.maKhachHang,
                    ChiTietPhieuDats = model.chiTietPhieuDats
                };

                _context.Add(phieuDat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, trả về view với model cũ
            ViewBag.maKhachHang = new SelectList(await _context.KhachHang.ToListAsync(), "maKhachHang", "tenKhachHang");
            return View();
        }

        // GET: PhieuDat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Controller"] = "PhieuDat";
            if (id == null)
            {
                return NotFound();
            }

            var phieuDat = await _context.PhieuDat
                .Include(p => p.KhachHang)
                .Include(p => p.ChiTietPhieuDats)
                .ThenInclude(c => c.ThietBi)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    maKhachHang = phieuDat.maKhachHang,
                    KhachHang = phieuDat.KhachHang,
                    ChiTietPhieuDats = phieuDat.ChiTietPhieuDats
                })
                .FirstOrDefaultAsync(m => m.maPhieuDat == id);

            if (phieuDat == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetInt32("maPhieuDat", phieuDat.maPhieuDat);
            // Tạo view model từ entity
            var model = new PhieuDatModel
            {
                maPhieuDat = phieuDat.maPhieuDat,
                diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                ghiChu = phieuDat.ghiChu,
                maKhachHang = phieuDat.maKhachHang,
                khachHang = phieuDat.KhachHang,
                chiTietPhieuDats = phieuDat.ChiTietPhieuDats.ToList()
            };

            ViewBag.maKhachHang = new SelectList(await _context.KhachHang.ToListAsync(), "maKhachHang", "tenKhachHang", phieuDat.maKhachHang);
            return View(model);
        }

        // POST: PhieuDat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhieuDatModel model)
        {
            if (id != model.maPhieuDat)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var phieuDat = await _context.PhieuDat
                                .Include(p => p.KhachHang)
                                .Include(p => p.ChiTietPhieuDats)
                                .Select(phieuDat => new PhieuDat
                                {
                                    maPhieuDat = phieuDat.maPhieuDat,
                                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                                    maKhachHang = phieuDat.maKhachHang,
                                    KhachHang = phieuDat.KhachHang,
                                    ChiTietPhieuDats = phieuDat.ChiTietPhieuDats
                                })
                                .FirstOrDefaultAsync(m => m.maPhieuDat == id);

                    if (phieuDat == null)
                    {
                        return NotFound();
                    }

                    phieuDat.diaCHiGiaoHang = model.diaCHiGiaoHang;
                    phieuDat.soDienThoaiGiaoHang = model.soDienThoaiGiaoHang;
                    phieuDat.ghiChu = model.ghiChu;
                    phieuDat.trangThaiPhieuDat = model.trangThaiPhieuDat;


                    _context.Update(phieuDat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuDatExists(model.maPhieuDat))
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

            ViewBag.maKhachHang = new SelectList(await _context.KhachHang.ToListAsync(), "maKhachHang", "tenKhachHang", model.maKhachHang);
            return View(model);
        }
        public IActionResult DuyetPhieuDat(int id)
        {
            var phieuDat = _context.PhieuDat
                .Include(p => p.KhachHang)
                .Include(p => p.ChiTietPhieuDats)
                .ThenInclude(c => c.ThietBi)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    maKhachHang = phieuDat.maKhachHang,
                    KhachHang = phieuDat.KhachHang,
                    ChiTietPhieuDats = phieuDat.ChiTietPhieuDats
                })
                .FirstOrDefault(m => m.maPhieuDat == id);
            if (phieuDat == null)
            {
                return NotFound();
            }

            return View(phieuDat);
        }
        [HttpPost]
        public IActionResult DuyetPhieuDat(int id, string actionType)
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDat = _context.PhieuDat
                .Include(p => p.KhachHang)
                .Include(p => p.ChiTietPhieuDats)
                .ThenInclude(c => c.ThietBi)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    maKhachHang = phieuDat.maKhachHang,
                    KhachHang = phieuDat.KhachHang,
                    ChiTietPhieuDats = phieuDat.ChiTietPhieuDats
                })
                .FirstOrDefault(m => m.maPhieuDat == id);
            if (phieuDat != null)
            {
                if (actionType == "Duyet")
                {
                    phieuDat.trangThaiPhieuDat = "Đã xác nhận";
                    _context.Update(phieuDat);
                    if (!User.Identity.IsAuthenticated)
                    {
                        return Unauthorized();
                    }
                    var maNhanVienClaim = User.FindFirst("maNhanVien");
                    if (maNhanVienClaim != null)
                    {
                        if (int.TryParse(maNhanVienClaim.Value, out int maNhanVien))
                        {
                            var donHang = new DonHang()
                            {
                                trangThaiDonHang = "Đang xử lý",
                                maPhieuDat = phieuDat.maPhieuDat,
                                PhieuDat = phieuDat,
                                maNhanVien = maNhanVien,
                            };
                            _context.Add(donHang);
                        }

                    }

                }
                else if (actionType == "Huy")
                {
                    phieuDat.trangThaiPhieuDat = "Đã hủy";
                    _context.Update(phieuDat);
                }

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDat = await _context.PhieuDat
                .Where(p => p.maPhieuDat == id)
                .Select(p => new PhieuDat
                {
                    maPhieuDat = p.maPhieuDat,
                    ngayTaoPhieu = p.ngayTaoPhieu,
                    trangThaiPhieuDat = p.trangThaiPhieuDat,
                    diaCHiGiaoHang = p.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = p.soDienThoaiGiaoHang,
                    ghiChu = p.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    KhachHang = p.KhachHang
                })
                .FirstOrDefaultAsync();

            if (phieuDat == null)
            {
                return NotFound();
            }

            _context.PhieuDat.Remove(phieuDat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string query)
        {
            ViewData["Controller"] = "PhieuDat";
            int queryAsInt;
            bool isIntQuery = int.TryParse(query, out queryAsInt);

            // Bắt đầu với truy vấn cơ bản
            var phieuDatQuery = _context.PhieuDat
                .Include(t => t.KhachHang)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    KhachHang = phieuDat.KhachHang
                })
                .AsQueryable();

            if (isIntQuery)
            {
                phieuDatQuery = phieuDatQuery.Where(tb => tb.maPhieuDat == queryAsInt || tb.soDienThoaiGiaoHang.Equals(query));
            }
            else
            {
                phieuDatQuery = phieuDatQuery.Where(tb => tb.KhachHang.tenKhachHang.Contains(query)
                                                  || tb.ghiChu.Contains(query)
                                                  || tb.soDienThoaiGiaoHang.Contains(query)
                                                  || tb.diaCHiGiaoHang.Contains(query));
            }


            var phieuDat = await phieuDatQuery.ToListAsync();
            return View("Index", phieuDat);
        }
        public async Task<IActionResult> Filter(string? trangThai, DateTime? start_taoPhieu, DateTime? end_taoPhieu)
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDat = await _context.PhieuDat
                .Include(t => t.KhachHang)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    KhachHang = phieuDat.KhachHang
                })
                .Where(tb => tb.trangThaiPhieuDat.Equals(trangThai) || (tb.ngayTaoPhieu >= start_taoPhieu && tb.ngayTaoPhieu <= end_taoPhieu))
                .ToListAsync();
            return View("Index", phieuDat);
        }
        public async Task<IActionResult> FilterDuyet()
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDat = await _context.PhieuDat
                .Include(t => t.KhachHang)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    KhachHang = phieuDat.KhachHang
                })
                .Where(tb => tb.trangThaiPhieuDat == "Chưa xác nhận")
                .ToListAsync();
            return View("Index", phieuDat);
        }

        //Biết đọc chữ k
        //Tìm kiếm của nhân viên
        public async Task<IActionResult> PhieudatSearchStaff(string query)
        {
            ViewData["Controller"] = "PhieuDat";
            int queryAsInt;
            bool isIntQuery = int.TryParse(query, out queryAsInt);

            // Bắt đầu với truy vấn cơ bản
            var phieuDatQuery = _context.PhieuDat
                .Include(t => t.KhachHang)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    KhachHang = phieuDat.KhachHang
                })
                .AsQueryable();

            if (isIntQuery)
            {
                phieuDatQuery = phieuDatQuery.Where(tb => tb.maPhieuDat == queryAsInt || tb.soDienThoaiGiaoHang.Equals(query));
            }
            else
            {
                phieuDatQuery = phieuDatQuery.Where(tb => tb.KhachHang.tenKhachHang.Contains(query)
                                                  || tb.soDienThoaiGiaoHang.Contains(query)
                                                  || tb.diaCHiGiaoHang.Contains(query));
            }


            var phieuDat = await phieuDatQuery.ToListAsync();
            return View("StaffIndex", phieuDat);
        }
        public async Task<IActionResult> PhieudatFilterStaff(string trangThai)
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDat = await _context.PhieuDat
                .Include(t => t.KhachHang)
                .Select(phieuDat => new PhieuDat
                {
                    maPhieuDat = phieuDat.maPhieuDat,
                    ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                    trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                    diaCHiGiaoHang = phieuDat.diaCHiGiaoHang,
                    soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                    ghiChu = phieuDat.ghiChu ?? "No notes", // Kiểm tra và xử lý giá trị null tại đây
                    KhachHang = phieuDat.KhachHang
                })
                .Where(tb => tb.trangThaiPhieuDat.Equals(trangThai))
                .ToListAsync();
            return View("StaffIndex", phieuDat);
        }
        private bool PhieuDatExists(int id)
        {
            return _context.PhieuDat.Any(e => e.maPhieuDat == id);
        }
    }
}
