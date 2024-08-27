using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.ViewModels
{
    public class PhieuDatModel
    {
        [Key]
        public int maPhieuDat { get; set; }
        [Required]
        [StringLength(150)]
        public string diaCHiGiaoHang { get; set; }

        public DateTime ngayTaoPhieu { get; set; }

        [StringLength(50)]
        public String trangThaiPhieuDat { get; set; }

        [StringLength(20)]
        public string soDienThoaiGiaoHang { get; set; }

        [StringLength(50)]
        public string? ghiChu { get; set; }

        [Required]
        public int maKhachHang { get; set; }

        public KhachHang? khachHang { get; set; }
        public ICollection<ChiTietPhieuDat> chiTietPhieuDats { get; set; } = new List<ChiTietPhieuDat>();
    }
}
