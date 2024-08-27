using System;

namespace WebChoThueThietBiXD.ViewModels
{
    public class QuanLyNhanVienViewModel
    {
        public int MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public decimal? TienLuong { get; set; }
        public string DiaChi { get; set; }
        public int MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; }
    }
}