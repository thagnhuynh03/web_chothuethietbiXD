using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebChoThueThietBiXD.ViewModels
{
    public class DanhMucThietBiViewModel
    {
        [Key]
        [Display(Name = "Mã Danh mục")]
        public int maDanhMuc { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên Danh mục")]
        public string tenDanhMuc { get; set; }

    }
}