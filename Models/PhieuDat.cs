using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class PhieuDat
    {
        [Key]
        public int maPhieuDat { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngayTaoPhieu { get; set; }

        [Required]
        [StringLength(50)]
        public String trangThaiPhieuDat { get; set; }

        [Required]
        [StringLength(150)]
        public String diaCHiGiaoHang { get; set; }

        [Required]
        [StringLength(20)]
        public string soDienThoaiGiaoHang { get; set; }

        [Required]
        [StringLength(50)]
        public string? ghiChu { get; set; }

        [ForeignKey("KhachHang")]
        public int maKhachHang { get; set; }
        public KhachHang? KhachHang { get; set; }

        public ICollection<ChiTietPhieuDat> ChiTietPhieuDats { get; set; } = new List<ChiTietPhieuDat>();
        public ICollection<DonHang> DonHangs { get; set; }
    }
}
