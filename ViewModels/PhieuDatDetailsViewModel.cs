using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.ViewModels
{
    public class PhieuDatDetailsViewModel
    {
        // Thông tin phiếu đặt
        public int MaPhieuDat { get; set; }
        public DateTime NgayTaoPhieu { get; set; }
        public string TrangThaiPhieuDat { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoaiGiaoHang { get; set; }
        public string? GhiChu { get; set; }
        public string KhachHang { get; set; }

        // Danh sách chi tiết phiếu đặt
        public ICollection<ChiTietPhieuDat> ChiTietPhieuDats { get; set; } = new List<ChiTietPhieuDat>();
    }
}
