using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.Models;
using WebChoThueThietBiXD.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebChoThueThietBiXD.Controllers
{
    [Authorize(Roles = "Nhân Viên, admin")]
    public class StaffController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public StaffController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Nếu không lấy được giá trị, thử lấy User.Identity.Name
            if (currentUserId == null)
            {
                currentUserId = User.Identity.Name;
            }

            var currentUser = await _context.TaiKhoan.SingleOrDefaultAsync(tk => tk.tenDangNhap == currentUserId);

            if (currentUser != null)
            {
                // Kiểm tra xem nhân viên có dữ liệu trong bảng NhanVien hay chưa
                var nhanVien = await _context.NhanVien.SingleOrDefaultAsync(nv => nv.maTaiKhoan == currentUser.maTaiKhoan);
                ViewBag.NhanVienExists = nhanVien != null;

                if (nhanVien == null)
                {
                    // Chuyển hướng đến CreateNhanVien nếu chưa có dữ liệu nhân viên
                    return RedirectToAction("CreateNhanVien");
                }

                // Nếu đã có dữ liệu nhân viên, truyền thông tin vào ViewBag
                ViewBag.NhanVien = nhanVien;

                // Nếu đã có dữ liệu, tiếp tục đến trang Staff/Index
                return View();
            }

            ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản hiện tại.");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> StaffProfile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
            {
                currentUserId = User.Identity.Name;
            }

            var currentUser = await _context.TaiKhoan.SingleOrDefaultAsync(tk => tk.tenDangNhap == currentUserId);
            if (currentUser == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanVien.SingleOrDefaultAsync(nv => nv.maTaiKhoan == currentUser.maTaiKhoan);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffProfile([Bind("maNhanVien,tenNhanVien,ngaySinh,Email,soDienThoai,tienLuong,diaChi")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingNhanVien = await _context.NhanVien.FindAsync(nhanVien.maNhanVien);
                    if (existingNhanVien == null)
                    {
                        return NotFound();
                    }

                    // Update the necessary fields only
                    existingNhanVien.tenNhanVien = nhanVien.tenNhanVien;
                    existingNhanVien.ngaySinh = nhanVien.ngaySinh;
                    existingNhanVien.Email = nhanVien.Email;
                    existingNhanVien.soDienThoai = nhanVien.soDienThoai;
                    existingNhanVien.tienLuong = nhanVien.tienLuong;
                    existingNhanVien.diaChi = nhanVien.diaChi;

                    _context.Update(existingNhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienExists(nhanVien.maNhanVien))
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
            return View(nhanVien);
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanVien.Any(e => e.maNhanVien == id);
        }
        // GET: Staff/CreateKhachHang
        [HttpGet]
        public IActionResult CreateKhachHang()
        {
            var viewModel = new KhachHangViewModel
            {
                VaiTros = new SelectList(_context.VaiTro, "maVaiTro", "tenVaiTro"),
                ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateKhachHang(KhachHangViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại hay chưa
                var existingAccount = await _context.TaiKhoan
                    .FirstOrDefaultAsync(tk => tk.tenDangNhap == viewModel.tenDangNhap);

                if (existingAccount != null)
                {
                    ModelState.AddModelError(string.Empty, "Tên người dùng đã tồn tại. Vui lòng chọn tên người dùng khác.");
                    viewModel.VaiTros = new SelectList(_context.VaiTro, "maVaiTro", "tenVaiTro");
                    viewModel.ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");
                    return View(viewModel);
                }

                // Tạo tài khoản
                var taiKhoan = new TaiKhoan
                {
                    tenDangNhap = viewModel.tenDangNhap,
                    matKhau = viewModel.matKhau,
                    maVaiTro = viewModel.maVaiTro
                };
                _context.TaiKhoan.Add(taiKhoan);
                await _context.SaveChangesAsync();

                // Tạo khách hàng
                var khachHang = new KhachHang
                {
                    tenKhachHang = viewModel.tenKhachHang,
                    ngaySinh = viewModel.ngaySinh,
                    Email = viewModel.Email,
                    soDienThoai = viewModel.soDienThoai,
                    diaChi = viewModel.diaChi,
                    maTaiKhoan = taiKhoan.maTaiKhoan
                };
                _context.KhachHang.Add(khachHang);
                await _context.SaveChangesAsync();

                // Tạo phiếu đặt
                var phieuDat = new PhieuDat
                {
                    ngayTaoPhieu = viewModel.ngayTaoPhieu,
                    trangThaiPhieuDat = viewModel.trangThaiPhieuDat,
                    diaCHiGiaoHang = viewModel.diaChiGiaoHang,
                    soDienThoaiGiaoHang = viewModel.soDienThoaiGiaoHang,
                    ghiChu = viewModel.ghiChu ?? "No notes",
                    maKhachHang = khachHang.maKhachHang
                };
                _context.PhieuDat.Add(phieuDat);
                await _context.SaveChangesAsync();

                // Tạo chi tiết phiếu đặt
                var chiTietPhieuDat = new ChiTietPhieuDat
                {
                    soLuongThue = viewModel.soLuongThue,
                    giaThueNgay = viewModel.giaThueNgay,
                    ngayBatDauThue = viewModel.ngayBatDauThue,
                    ngayKetThucThue = viewModel.ngayKetThucThue,
                    maThietBi = viewModel.maThietBi,
                    maPhieuDat = phieuDat.maPhieuDat
                };
                _context.ChiTietPhieuDat.Add(chiTietPhieuDat);
                await _context.SaveChangesAsync();

                return RedirectToAction("StaffIndex", "PhieuDat");
            }

            viewModel.VaiTros = new SelectList(_context.VaiTro, "maVaiTro", "tenVaiTro");
            viewModel.ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreatePhieuDat()
        {
            var viewModel = new PhieuDatViewModel
            {
                KhachHangs = new SelectList(_context.KhachHang, "maKhachHang", "tenKhachHang"),
                ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi")
            };
            return View(viewModel);
        }

        // POST: Staff/CreatePhieuDat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhieuDat(PhieuDatViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Tạo phiếu đặt
                var phieuDat = new PhieuDat
                {
                    ngayTaoPhieu = viewModel.ngayTaoPhieu,
                    trangThaiPhieuDat = viewModel.trangThaiPhieuDat,
                    diaCHiGiaoHang = viewModel.diaChiGiaoHang,
                    soDienThoaiGiaoHang = viewModel.soDienThoaiGiaoHang,
                    ghiChu = viewModel.ghiChu ?? "No notes",
                    maKhachHang = viewModel.maKhachHang
                };
                _context.PhieuDat.Add(phieuDat);
                await _context.SaveChangesAsync();

                // Tạo chi tiết phiếu đặt
                var chiTietPhieuDat = new ChiTietPhieuDat
                {
                    soLuongThue = viewModel.soLuongThue,
                    giaThueNgay = viewModel.giaThueNgay,
                    ngayBatDauThue = viewModel.ngayBatDauThue,
                    ngayKetThucThue = viewModel.ngayKetThucThue,
                    maThietBi = viewModel.maThietBi,
                    maPhieuDat = phieuDat.maPhieuDat
                };
                _context.ChiTietPhieuDat.Add(chiTietPhieuDat);
                await _context.SaveChangesAsync();

                return RedirectToAction("StaffIndex", "PhieuDat");
            }

            viewModel.KhachHangs = new SelectList(_context.KhachHang, "maKhachHang", "tenKhachHang");
            viewModel.ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");

            return View(viewModel);
        }

        // GET: Staff/UpdatePhieuDat/5
        [HttpGet]
        public async Task<IActionResult> UpdatePhieuDat(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDat = await _context.PhieuDat
                .Include(pd => pd.ChiTietPhieuDats)
                .FirstOrDefaultAsync(pd => pd.maPhieuDat == id);

            if (phieuDat == null)
            {
                return NotFound();
            }

            var chiTietPhieuDat = phieuDat.ChiTietPhieuDats.FirstOrDefault();
            var viewModel = new PhieuDatViewModel
            {
                ngayTaoPhieu = phieuDat.ngayTaoPhieu,
                trangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                diaChiGiaoHang = phieuDat.diaCHiGiaoHang,
                soDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                ghiChu = phieuDat.ghiChu ?? "No notes",
                maKhachHang = phieuDat.maKhachHang,
                soLuongThue = chiTietPhieuDat?.soLuongThue ?? 0,
                giaThueNgay = chiTietPhieuDat?.giaThueNgay ?? 0,
                ngayBatDauThue = chiTietPhieuDat?.ngayBatDauThue ?? DateTime.MinValue,
                ngayKetThucThue = chiTietPhieuDat?.ngayKetThucThue ?? DateTime.MinValue,
                maThietBi = chiTietPhieuDat?.maThietBi ?? 0,
                KhachHangs = new SelectList(_context.KhachHang, "maKhachHang", "tenKhachHang", phieuDat.maKhachHang),
                ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", chiTietPhieuDat?.maThietBi)
            };

            ViewBag.MaPhieuDat = id; // Store the id in ViewBag for the POST action

            return View(viewModel);
        }

        // POST: Staff/UpdatePhieuDat/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhieuDat(int id, PhieuDatViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var phieuDat = await _context.PhieuDat
                        .Include(pd => pd.ChiTietPhieuDats)
                        .FirstOrDefaultAsync(pd => pd.maPhieuDat == id);

                    if (phieuDat == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin phiếu đặt
                    phieuDat.ngayTaoPhieu = viewModel.ngayTaoPhieu;
                    phieuDat.trangThaiPhieuDat = viewModel.trangThaiPhieuDat;
                    phieuDat.diaCHiGiaoHang = viewModel.diaChiGiaoHang;
                    phieuDat.soDienThoaiGiaoHang = viewModel.soDienThoaiGiaoHang;
                    phieuDat.ghiChu = viewModel.ghiChu ?? "No notes";
                    phieuDat.maKhachHang = viewModel.maKhachHang;

                    // Cập nhật chi tiết phiếu đặt
                    var chiTietPhieuDat = phieuDat.ChiTietPhieuDats.FirstOrDefault();
                    if (chiTietPhieuDat != null)
                    {
                        chiTietPhieuDat.soLuongThue = viewModel.soLuongThue;
                        chiTietPhieuDat.giaThueNgay = viewModel.giaThueNgay;
                        chiTietPhieuDat.ngayBatDauThue = viewModel.ngayBatDauThue;
                        chiTietPhieuDat.ngayKetThucThue = viewModel.ngayKetThucThue;
                        chiTietPhieuDat.maThietBi = viewModel.maThietBi;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhieuDatExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("StaffIndex", "PhieuDat");
            }

            viewModel.KhachHangs = new SelectList(_context.KhachHang, "maKhachHang", "tenKhachHang", viewModel.maKhachHang);
            viewModel.ThietBis = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", viewModel.maThietBi);

            ViewBag.MaPhieuDat = id; // Store the id in ViewBag in case of model validation failure

            return View(viewModel);
        }

        private bool PhieuDatExists(int id)
        {
            return _context.PhieuDat.Any(e => e.maPhieuDat == id);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePhieuDat(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuDat = await _context.PhieuDat
                .Include(p => p.KhachHang)
                .FirstOrDefaultAsync(m => m.maPhieuDat == id);

            if (phieuDat == null)
            {
                return NotFound();
            }

            return View(phieuDat);
        }

        [HttpPost, ActionName("DeletePhieuDat")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phieuDat = await _context.PhieuDat.FindAsync(id);

            if (phieuDat == null)
            {
                return NotFound();
            }

            _context.PhieuDat.Remove(phieuDat);
            await _context.SaveChangesAsync();
            return RedirectToAction("StaffIndex", "PhieuDat");
        }

        public IActionResult DetailsPhieuDat(int id)
        {
            var phieuDat = _context.PhieuDat
                .Include(pd => pd.KhachHang)
                .Include(pd => pd.ChiTietPhieuDats)
                .ThenInclude(ctpd => ctpd.ThietBi)
                .FirstOrDefault(pd => pd.maPhieuDat == id);

            if (phieuDat == null)
            {
                return NotFound();
            }

            var viewModel = new PhieuDatDetailsViewModel
            {
                MaPhieuDat = phieuDat.maPhieuDat,
                NgayTaoPhieu = phieuDat.ngayTaoPhieu,
                TrangThaiPhieuDat = phieuDat.trangThaiPhieuDat,
                DiaChiGiaoHang = phieuDat.diaCHiGiaoHang,
                SoDienThoaiGiaoHang = phieuDat.soDienThoaiGiaoHang,
                GhiChu = phieuDat.ghiChu ?? "No notes",
                KhachHang = phieuDat.KhachHang.tenKhachHang,
                ChiTietPhieuDats = phieuDat.ChiTietPhieuDats.ToList() // Sử dụng trực tiếp ChiTietPhieuDat
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateChiTietPhieuDat(int maPhieuDat)
        {
            // Truy vấn dữ liệu để lấy tên khách hàng
            var phieuDatList = _context.PhieuDat
                .Include(pd => pd.KhachHang)
                .Select(pd => new {
                    maPhieuDat = pd.maPhieuDat,
                    tenKhachHang = pd.KhachHang.tenKhachHang
                }).ToList();
            ViewData["maPhieuDat"] = maPhieuDat; // Đặt maPhieuDat vào ViewData
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChiTietPhieuDat([Bind("maChiTietPhieuDat,soLuongThue,giaThueNgay,ngayBatDauThue,ngayKetThucThue,maPhieuDat,maThietBi")] ChiTietPhieuDat chiTietPhieuDat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietPhieuDat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsPhieuDat), new { id = chiTietPhieuDat.maPhieuDat });
            }

            // Nếu ModelState không hợp lệ, cần phải nạp lại ViewBag dữ liệu cho các dropdown
            var phieuDatList = _context.PhieuDat
                .Include(pd => pd.KhachHang)
                .Select(pd => new {
                    maPhieuDat = pd.maPhieuDat,
                    tenKhachHang = pd.KhachHang.tenKhachHang
                }).ToList();
            ViewData["maPhieuDat"] = new SelectList(phieuDatList, "maPhieuDat", "tenKhachHang", chiTietPhieuDat.maPhieuDat);
            ViewData["maThietBi"] = new SelectList(_context.ThietBi, "maThietBi", "tenThietBi", chiTietPhieuDat.maThietBi);
            return View(chiTietPhieuDat);
        }

        // GET: Staff/DeleteChiTietPhieuDat/5
        public async Task<IActionResult> DeleteChiTietPhieuDat(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietPhieuDat = await _context.ChiTietPhieuDat
                .Include(c => c.ThietBi)
                .Include(c => c.PhieuDat)
                .FirstOrDefaultAsync(m => m.maChiTietPhieuDat == id);
            if (chiTietPhieuDat == null)
            {
                return NotFound();
            }

            return View(chiTietPhieuDat);
        }

        // POST: Staff/DeleteChiTietPhieuDat/5
        [HttpPost, ActionName("DeleteChiTietPhieuDat")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteChiTietPhieuDatConfirmed(int id)
        {
            var chiTietPhieuDat = await _context.ChiTietPhieuDat.FindAsync(id);
            if (chiTietPhieuDat != null)
            {
                _context.ChiTietPhieuDat.Remove(chiTietPhieuDat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DetailsPhieuDat), new { id = chiTietPhieuDat.maPhieuDat });
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult CreateNhanVien()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNhanVien([Bind("maNhanVien,tenNhanVien,ngaySinh,Email,soDienThoai,tienLuong,diaChi")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                // Thử dùng HttpContext.User trực tiếp
                var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Nếu không lấy được giá trị, thử lấy User.Identity.Name
                if (currentUserId == null)
                {
                    currentUserId = HttpContext.User.Identity.Name;
                }

                ViewBag.CurrentUserId = currentUserId; // Hiển thị giá trị này trong View để kiểm tra

                var currentUser = await _context.TaiKhoan.SingleOrDefaultAsync(tk => tk.tenDangNhap == currentUserId);

                if (currentUser != null)
                {
                    nhanVien.maTaiKhoan = currentUser.maTaiKhoan;
                    _context.Add(nhanVien);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản hiện tại.");
            }
            return View(nhanVien);
        }

        public async Task<IActionResult> DetailsDonHang(int maDonHang)
        {
            var donHang = await _context.DonHang
                .Include(dh => dh.PhieuDat)
                .ThenInclude(pd => pd.KhachHang)
                .Include(dh => dh.PhieuDat.ChiTietPhieuDats)
                .ThenInclude(ctpd => ctpd.ThietBi)
                .Include(dh => dh.NhanVien)
                .FirstOrDefaultAsync(dh => dh.maDonHang == maDonHang);

            if (donHang == null)
            {
                return NotFound();
            }

            var phieuDat = donHang.PhieuDat;

            var viewModel = new DonHangDetailsViewModel
            {
                PhieuDat = phieuDat,
                ChiTietPhieuDats = phieuDat.ChiTietPhieuDats.ToList(),
                DonHangs = new List<DonHang> { donHang }
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UpdateDonHang(int maDonHang)
        {
            var donHang = await _context.DonHang
                .Include(dh => dh.NhanVien)
                .FirstOrDefaultAsync(dh => dh.maDonHang == maDonHang);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDonHang(DonHang donHang)
        {
            if (!ModelState.IsValid)
            {
                return View(donHang);
            }

            var existingDonHang = await _context.DonHang.FindAsync(donHang.maDonHang);

            if (existingDonHang == null)
            {
                return NotFound();
            }

            // Update the fields
            existingDonHang.trangThaiDonHang = donHang.trangThaiDonHang;
            existingDonHang.ngayThanhToan = donHang.ngayThanhToan;
            existingDonHang.ngayGiaoHang = donHang.ngayGiaoHang;

            // Save the changes to the database
            _context.Update(existingDonHang);
            await _context.SaveChangesAsync();

            // Redirect to the DetailsDonHang action
            return RedirectToAction(nameof(DetailsDonHang), new { maDonHang = existingDonHang.maDonHang });
        }

        public IActionResult DuyetPhieuDat(string status)
        {
            var orders = _context.PhieuDat.Include(o => o.KhachHang).AsQueryable();

            switch (status)
            {
                case "ChuaXacNhan":
                    orders = orders.Where(o => o.trangThaiPhieuDat == "Chưa xác nhận");
                    break;
                case "DaXacNhan":
                    orders = orders.Where(o => o.trangThaiPhieuDat == "Đã xác nhận");
                    break;
                case "DaHuy":
                    orders = orders.Where(o => o.trangThaiPhieuDat == "Đã hủy");
                    break;
                default:
                    orders = orders.Where(o => o.trangThaiPhieuDat == "Chưa xác nhận");
                    break;
            }

            return View(orders.ToList());
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int id, string action)
        {
            var order = _context.PhieuDat.Find(id);
            if (order == null) return NotFound();

            if (action == "confirm")
            {
                order.trangThaiPhieuDat = "Đã xác nhận";
            }
            else if (action == "cancel")
            {
                order.trangThaiPhieuDat = "Đã hủy";
            }
            else if (action == "undo")
            {
                order.trangThaiPhieuDat = "Chưa xác nhận";
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(DuyetPhieuDat));
        }

        public async Task<IActionResult> Search(string query)
        {
            ViewData["Controller"] = "PhieuDat";
            int queryAsInt;
            bool isIntQuery = int.TryParse(query, out queryAsInt);

            var phieuDatQuery = _context.PhieuDat
                .Include(t => t.KhachHang)
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
        public async Task<IActionResult> Filter(string trangThai)
        {
            ViewData["Controller"] = "PhieuDat";
            var phieuDatQuery = _context.PhieuDat
                .Include(t => t.KhachHang)
                .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
            {
                phieuDatQuery = phieuDatQuery.Where(tb => tb.trangThaiPhieuDat.Equals(trangThai));
            }

            var phieuDat = await phieuDatQuery.ToListAsync();
            return View("Index", phieuDat);
        }
    }
}
