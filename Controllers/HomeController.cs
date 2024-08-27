using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebChoThueThietBiXDContext _context;

        public HomeController(ILogger<HomeController> logger, WebChoThueThietBiXDContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect based on user role
                if (User.IsInRole("admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Nhân Viên"))
                {
                    return RedirectToAction("Index", "Staff");
                }
            }
            var danhSachSanPhams = _context.ThietBi.ToList();
            ViewData["danhSachSanPhams"] = danhSachSanPhams; // Truyền dữ liệu vào ViewData
            ViewData["maVaiTro"] = new SelectList(_context.VaiTro, "maVaiTro", "tenVaiTro");

 
            return View();

        }

        [Route("Profile/{id?}")]
        public IActionResult Profile(string id)
        {
            var khachHang = _context.KhachHang.Find(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }


        [HttpPost]
        public async Task<IActionResult> Index(TaiKhoan taiKhoan, string actionType, string confirmPassword)
        {
            if (actionType == "login")
            {
                // Đăng nhập
                var _user = await _context.TaiKhoan.Include(u => u.VaiTro).Include(u => u.NhanViens)
                    .FirstOrDefaultAsync(m => m.tenDangNhap == taiKhoan.tenDangNhap && m.matKhau == taiKhoan.matKhau);

                if (_user == null)
                {
                    ViewBag.LoginStatus = 0;
                }
                else
                {
                    var nhanVien = _user.NhanViens.FirstOrDefault();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _user.tenDangNhap),
                        new Claim(ClaimTypes.Role, _user.VaiTro.tenVaiTro),
                        new Claim("maNhanVien", nhanVien?.maNhanVien.ToString() ?? string.Empty)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirect based on user role
                    switch (_user.VaiTro.tenVaiTro)
                    {
                        case "admin":
                            return RedirectToAction("Index", "Admin");
                        case "Nhân Viên":
                            return RedirectToAction("Index", "Staff");
                        case "Người dùng":
                            return RedirectToAction("Index", "Home");
                        default:
                            return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
            else if (actionType == "register")
            {
                // Đăng ký
                if (taiKhoan.matKhau != confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "Mật khẩu xác nhận không khớp.");
                }

                var existingUser = await _context.TaiKhoan.FirstOrDefaultAsync(u => u.tenDangNhap == taiKhoan.tenDangNhap);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Tên người dùng đã tồn tại. Vui lòng chọn tên người dùng khác.");
                }

                if (ModelState.IsValid)
                {
                    _context.Add(taiKhoan);
                    await _context.SaveChangesAsync();
                    ViewBag.RegistrationSuccess = true;
                    return RedirectToAction(nameof(Index));
                }
                ViewData["maVaiTro"] = new SelectList(_context.VaiTro, "maVaiTro", "tenVaiTro", taiKhoan.maVaiTro);
                ViewBag.RegistrationSuccess = false;
                return View(taiKhoan);
            }
            return View();
        }


        public IActionResult AddToCart(int maTB)
        {

            var userName = User.Identity.Name;
            var user = _context.TaiKhoan.FirstOrDefault(u => u.tenDangNhap == userName);

            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var existingCartItem = _context.GioHang
                .FirstOrDefault(c => c.maThietBi == maTB && c.maTaiKhoan == user.maTaiKhoan);
            var cartCount = _context.GioHang.Count(c => c.maTaiKhoan == user.maTaiKhoan);
            if (existingCartItem != null)
            {
                return Json(new { success = true, cartCount = cartCount });
            }
            var gioHang = new GioHang
            {
                maThietBi = maTB,
                maTaiKhoan = user.maTaiKhoan
            };

            _context.GioHang.Add(gioHang);
            _context.SaveChanges();


            return Json(new { success = true, cartCount = cartCount + 1, message = "Sản phẩm đã được thêm vào giỏ hàng!" });
        }

        public IActionResult CartPreview()
        {
            var userName = User.Identity.Name;
            var user = _context.TaiKhoan.FirstOrDefault(u => u.tenDangNhap == userName);
            var gioHang = new List<WebChoThueThietBiXD.Models.GioHang>();
            if (user != null)
            {
                gioHang = _context.GioHang
                    .Include(g => g.ThietBi) // Ensure 'Include' is recognized and ThietBi is the correct navigation property
                    .Where(t => t.maTaiKhoan == user.maTaiKhoan)
                    .ToList();
            }

            return PartialView("_CartPreview", gioHang);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Category(string maDM, string maHang, string sortOrder)
        {
            var danhSachSanPhams = _context.ThietBi.AsQueryable();

            if (!string.IsNullOrEmpty(maDM))
            {
                var danhMucIds = maDM.Split(',').Select(int.Parse).ToList();
                danhSachSanPhams = danhSachSanPhams.Where(t => danhMucIds.Contains(t.maDanhMuc));
            }

            if (!string.IsNullOrEmpty(maHang))
            {
                var hangIds = maHang.Split(',').Select(int.Parse).ToList();
                danhSachSanPhams = danhSachSanPhams.Where(t => hangIds.Contains(t.maHang));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name-asc":
                    danhSachSanPhams = danhSachSanPhams.OrderBy(t => t.tenThietBi);
                    break;
                case "name-desc":
                    danhSachSanPhams = danhSachSanPhams.OrderByDescending(t => t.tenThietBi);
                    break;
                case "price-asc":
                    danhSachSanPhams = danhSachSanPhams.OrderBy(t => t.giaThue);
                    break;
                case "price-desc":
                    danhSachSanPhams = danhSachSanPhams.OrderByDescending(t => t.giaThue);
                    break;
                default:
                    break;
            }

            var sanPhams = await danhSachSanPhams.ToListAsync();
            ViewData["danhSachSanPhams"] = sanPhams;

            var categories = await _context.DanhMucThietBi.ToListAsync();
            var brands = await _context.Hang.ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.SelectedCategories = maDM;
            ViewBag.SelectedBrands = maHang;
            ViewBag.SortOrder = sortOrder;

            return View("Product");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Back()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
