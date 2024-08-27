using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class DonHang
    {
        [Key]
        public int maDonHang { get; set; }


        [Required]
        [StringLength(50)]
        public String trangThaiDonHang { get; set; }

        [Required]
        public DateTime? ngayThanhToan { get; set; }

        [Required]
        public DateTime? ngayGiaoHang { get; set; }

        [ForeignKey("PhieuDat")]
        public int maPhieuDat { get; set; }
        public PhieuDat PhieuDat { get; set; }

        [ForeignKey("NhanVien")]
        public int maNhanVien { get; set; }
        public NhanVien NhanVien { get; set; }
    }
}
