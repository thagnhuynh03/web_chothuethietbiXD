using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebChoThueThietBiXD.Models;

namespace WebChoThueThietBiXD.Data
{
    public class WebChoThueThietBiXDContext : DbContext
    {
        public WebChoThueThietBiXDContext (DbContextOptions<WebChoThueThietBiXDContext> options)
            : base(options)
        {
        }

        public DbSet<WebChoThueThietBiXD.Models.DanhMucThietBi> DanhMucThietBi { get; set; }

        public DbSet<WebChoThueThietBiXD.Models.ThietBi> ThietBi { get; set; }

        public DbSet<WebChoThueThietBiXD.Models.HinhAnhThietBi> HinhAnhThietBi { get; set; }

        public DbSet<WebChoThueThietBiXD.Models.VaiTro> VaiTro { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.TaiKhoan> TaiKhoan { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.NhanVien> NhanVien { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.KhachHang> KhachHang { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.GioHang> GioHang { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.PhieuDat> PhieuDat { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.ChiTietPhieuDat> ChiTietPhieuDat { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.DonHang> DonHang { get; set; }



        public DbSet<WebChoThueThietBiXD.Models.Hang> Hang { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình khóa chính hỗn hợp cho GioHang
            modelBuilder.Entity<GioHang>()
                .HasKey(gh => new { gh.maThietBi, gh.maTaiKhoan });

            // Cấu hình ràng buộc khóa ngoại
            modelBuilder.Entity<GioHang>()
                .HasOne(gh => gh.ThietBi)
                .WithMany(tb => tb.GioHangs)
                .HasForeignKey(gh => gh.maThietBi);

            modelBuilder.Entity<GioHang>()
                .HasOne(gh => gh.TaiKhoan)
                .WithMany(tk => tk.GioHangs)
                .HasForeignKey(gh => gh.maTaiKhoan);

            // Configure relationships for ChiTietPhieuDat
            modelBuilder.Entity<ChiTietPhieuDat>()
                .HasOne(ctpd => ctpd.PhieuDat)
                .WithMany(pd => pd.ChiTietPhieuDats)
                .HasForeignKey(ctpd => ctpd.maPhieuDat);

            modelBuilder.Entity<ChiTietPhieuDat>()
                .HasOne(ctpd => ctpd.ThietBi)
                .WithMany(tb => tb.ChiTietPhieuDats)
                .HasForeignKey(ctpd => ctpd.maThietBi);
            modelBuilder.Entity<ChiTietPhieuDat>()
                .Property(ctpd => ctpd.giaThueNgay)
                .HasPrecision(18, 2);

            // Configure relationships for DonHang
            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.PhieuDat)
                .WithMany(pd => pd.DonHangs)
                .HasForeignKey(dh => dh.maPhieuDat);

            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.NhanVien)
                .WithMany(nv => nv.DonHangs)
                .HasForeignKey(dh => dh.maNhanVien);

            // Đảm bảo rằng bảng ThietBi tham chiếu đến các id Hang hợp lệ
            modelBuilder.Entity<ThietBi>()
                .HasOne(tb => tb.Hang)
                .WithMany(h => h.ThietBis)
                .HasForeignKey(tb => tb.maHang)
                .OnDelete(DeleteBehavior.Restrict);  // Điều chỉnh hành vi xóa nếu cần thiết
            modelBuilder.Entity<ThietBi>()
               .Property(tb => tb.giaThue)
               .HasPrecision(18, 2);

            modelBuilder.Entity<NhanVien>()
                .Property(nv => nv.tienLuong)
                .HasPrecision(18, 2);

        }


    }
}
