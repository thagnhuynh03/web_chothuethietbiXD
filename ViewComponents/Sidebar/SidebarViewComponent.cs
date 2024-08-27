using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChoThueThietBiXD.Data;
using WebChoThueThietBiXD.ViewComponents;

namespace WebChoThueThietBiXD.ViewComponents.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly WebChoThueThietBiXDContext _context;

        public SidebarViewComponent(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? HttpContext.User.Identity.Name;
            var currentUser = await _context.TaiKhoan.SingleOrDefaultAsync(tk => tk.tenDangNhap == currentUserId);

            if (currentUser != null)
            {
                var nhanVien = await _context.NhanVien.SingleOrDefaultAsync(nv => nv.maTaiKhoan == currentUser.maTaiKhoan);
                ViewBag.NhanVienExists = nhanVien != null;
            }
            else
            {
                ViewBag.NhanVienExists = false;
            }

            return View();
        }
    }
}
