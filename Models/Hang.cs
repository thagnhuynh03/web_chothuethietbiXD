using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.Models
{
    public class Hang
    {
        [Key]
        public int maHang { get; set; }


        [Required]
        [StringLength(50)]
        public String tenHang { get; set; }

        public ICollection<ThietBi> ThietBis { get; set; } = new List<ThietBi>();
    }
}
