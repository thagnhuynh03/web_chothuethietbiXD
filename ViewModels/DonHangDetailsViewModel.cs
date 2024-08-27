using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.ViewModels
{
    public class DonHangDetailsViewModel
    {
        public PhieuDat PhieuDat { get; set; }
        public List<ChiTietPhieuDat> ChiTietPhieuDats { get; set; }
        public List<DonHang> DonHangs { get; set; }
    }
}
