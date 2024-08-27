using Microsoft.AspNetCore.Mvc;
using WebChoThueThietBiXD.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebChoThueThietBiXD.Data;


namespace WebChoThueThietBiXD.Controllers
{
    public class product_detailController : Controller
    {
        private readonly WebChoThueThietBiXDContext _context;

        public product_detailController(WebChoThueThietBiXDContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult product_detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var thietBi = _context.ThietBi
                .Include(tb => tb.DanhMucThietBi)
                .Include(tb => tb.Hang)
                .Include(tb => tb.HinhAnhThietBis)
                .FirstOrDefault(tb => tb.maThietBi == id);

            if (thietBi == null)
            {
                return NotFound();
            }

            var danhSachThietBi = _context.ThietBi
                .Where(tb => tb.maDanhMuc == thietBi.maDanhMuc).ToList();
            ViewData["danhSachSanPhams"] = danhSachThietBi;
            var danhsachhinhanh = _context.HinhAnhThietBi.ToList();
            ViewData["danhsachhinhanh"] = danhsachhinhanh;  
            return View(thietBi);
        }
    }
}
