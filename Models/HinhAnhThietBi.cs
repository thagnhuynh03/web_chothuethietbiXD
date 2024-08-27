using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class HinhAnhThietBi
    {
        [Key]
        public int maHinhAnh { get; set; }

        [Required]
        [StringLength(255)]
        public String hinhAnh { get; set; }

        [ForeignKey("ThietBi")]
        public int maThietBi { get; set; }
        public ThietBi? ThietBi { get; set; }
    }
}
