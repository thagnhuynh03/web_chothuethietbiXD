using System;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.ViewModels
{
    public class ChiTietPhieuDatModel
    {
        public int maChiTietPhieuDat { get; set; }
        public ThietBi ThietBi { get; set; }
        public string tenThietBi { get; set; }
        public int soLuongThue { get; set; }
        public decimal giaThueNgay { get; set; }
        public DateTime ngayBatDauThue { get; set; }
        public DateTime ngayKetThucThue { get; set; }
    }
}
