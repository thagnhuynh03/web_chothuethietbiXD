using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebChoThueThietBiXD.ViewModels
{
    public class KhachHangViewModel
    {
        // Thông tin tài khoản
        public int maTaiKhoan { get; set; }
        public string tenDangNhap { get; set; }
        public string matKhau { get; set; }
        public int maVaiTro { get; set; }
        public SelectList VaiTros { get; set; } // Thêm SelectList

        // Thông tin khách hàng
        public string tenKhachHang { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngaySinh { get; set; }
        public string Email { get; set; }
        public string soDienThoai { get; set; }
        public string diaChi { get; set; }

        // Thông tin phiếu đặt
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayTaoPhieu { get; set; }
        public string trangThaiPhieuDat { get; set; }
        public string diaChiGiaoHang { get; set; }
        public string soDienThoaiGiaoHang { get; set; }
        public string? ghiChu { get; set; }

        // Thông tin chi tiết phiếu đặt
        public int soLuongThue { get; set; }
        public decimal giaThueNgay { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayBatDauThue { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayKetThucThue { get; set; }
        public int maThietBi { get; set; }
        public SelectList ThietBis { get; set; } // Thêm SelectList
    }
}