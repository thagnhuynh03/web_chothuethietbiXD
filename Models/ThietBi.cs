using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class ThietBi
    {
        [Key]
        public int maThietBi { get; set; }

        [Required]
        [StringLength(100)]
        public String tenThietBi { get; set; }

        [Required]
        [StringLength(250)]
        public String moTa { get; set; }

        [Required]
        public decimal giaThue { get; set; }

        [Required]
        public int soLuongCon { get; set; }

        [ForeignKey("DanhMucThietBi")]
        public int maDanhMuc { get; set; }
        public DanhMucThietBi? DanhMucThietBi { get; set; }

        [ForeignKey("Hang")]
        public int maHang { get; set; }
        public Hang? Hang { get; set; }

        public ICollection<HinhAnhThietBi> HinhAnhThietBis { get; set; } = new List<HinhAnhThietBi>();
        public ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
        public ICollection<ChiTietPhieuDat> ChiTietPhieuDats { get; set; } = new List<ChiTietPhieuDat>();
    }
}
