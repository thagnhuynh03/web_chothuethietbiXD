using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChoThueThietBiXD.ViewModels
{
    public class NhanVienViewModel
    {
        public int maNhanVien { get; set; }

        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        public string tenNhanVien { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ngaySinh { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string soDienThoai { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string diaChi { get; set; }

        [Required(ErrorMessage = "Tiền lương là bắt buộc.")]
        public decimal tienLuong { get; set; }
    }
}
