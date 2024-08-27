using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class ChiTietPhieuDat
    {
        [Key]
        public int maChiTietPhieuDat { get; set; }

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

        [ForeignKey("PhieuDat")]
        public int maPhieuDat { get; set; }
        public PhieuDat PhieuDat { get; set; }

        [ForeignKey("ThietBi")]
        public int maThietBi { get; set; }
        public ThietBi ThietBi { get; set; }
    }
}
