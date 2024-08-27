using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class TaiKhoan
    {
        [Key]
        public int maTaiKhoan { get; set; }

        [Required]
        [StringLength(50)]
        public String tenDangNhap { get; set; }

        [Required]
        [StringLength(20)]
        public String matKhau { get; set; }

        [ForeignKey("VaiTro")]
        public int maVaiTro { get; set; }
        public VaiTro? VaiTro { get; set; }

        public ICollection<KhachHang> khachHangs { get; set; } 
        public ICollection<NhanVien> NhanViens { get; set; }
        public ICollection<GioHang> GioHangs { get; set; }
    }
}
