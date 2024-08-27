using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class KhachHang
    {
        [Key]
        public int maKhachHang { get; set; }

        [Required]
        [StringLength(70)]
        public String tenKhachHang { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngaySinh { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string soDienThoai { get; set; }

        [Required]
        [StringLength(150)]
        public String diaChi { get; set; }

        [ForeignKey("TaiKhoan")]
        public int maTaiKhoan { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }

        public ICollection<PhieuDat> PhieuDats { get; set; } = new List<PhieuDat>();
    }
}
