using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class VaiTro
    {
        [Key]
        public int maVaiTro { get; set; }

        [Required]
        [StringLength(50)]
        public String tenVaiTro { get; set; }

        public ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
    }
}
