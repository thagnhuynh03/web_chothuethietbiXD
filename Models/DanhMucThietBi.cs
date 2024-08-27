using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class DanhMucThietBi
    {
        [Key]
        public int maDanhMuc { get; set; }

        [Required]
        [StringLength(100)]
        public String tenDanhMuc { get; set; }

        public ICollection<ThietBi> ThietBis { get; set; } = new List<ThietBi>();
    }
}
