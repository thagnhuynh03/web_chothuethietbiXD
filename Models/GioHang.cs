using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class GioHang
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("ThietBi")]
        public int maThietBi { get; set; }
        public ThietBi? ThietBi { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("TaiKhoan")]
        public int maTaiKhoan { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }
    }
}
