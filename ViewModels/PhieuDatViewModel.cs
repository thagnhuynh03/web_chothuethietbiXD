using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebChoThueThietBiXD.ViewModels
{
    public class PhieuDatViewModel
    {
        // Thông tin phiếu đặt
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayTaoPhieu { get; set; }

        [Required]
        [StringLength(50)]
        public string trangThaiPhieuDat { get; set; } = "Chưa xác nhận";

        [Required]
        [StringLength(150)]
        public string diaChiGiaoHang { get; set; }

        [Required]
        [StringLength(20)]
        public string soDienThoaiGiaoHang { get; set; }

        [StringLength(50)]
        public string? ghiChu { get; set; }

        [Required]
        public int maKhachHang { get; set; }

        // Thông tin chi tiết phiếu đặt
        [Required]
        public int soLuongThue { get; set; }

        [Required]
        public decimal giaThueNgay { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayBatDauThue { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayKetThucThue { get; set; }

        [Required]
        public int maThietBi { get; set; }

        
        public SelectList KhachHangs { get; set; }
        public SelectList ThietBis { get; set; }
    }
}