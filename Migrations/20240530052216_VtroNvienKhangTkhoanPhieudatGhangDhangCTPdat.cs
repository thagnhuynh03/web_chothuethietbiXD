using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChoThueThietBiXD.Migrations
{
    public partial class VtroNvienKhangTkhoanPhieudatGhangDhangCTPdat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VaiTro",
                columns: table => new
                {
                    maVaiTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTro", x => x.maVaiTro);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    maTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    matKhau = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    maVaiTro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.maTaiKhoan);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_VaiTro_maVaiTro",
                        column: x => x.maVaiTro,
                        principalTable: "VaiTro",
                        principalColumn: "maVaiTro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    maThietBi = table.Column<int>(type: "int", nullable: false),
                    maTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => new { x.maThietBi, x.maTaiKhoan });
                    table.ForeignKey(
                        name: "FK_GioHang_TaiKhoan_maTaiKhoan",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHang_ThietBi_maThietBi",
                        column: x => x.maThietBi,
                        principalTable: "ThietBi",
                        principalColumn: "maThietBi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    maKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenKhachHang = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    ngaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    soNha = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    maTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.maKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_TaiKhoan_maTaiKhoan",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    maNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenNhanVien = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    ngaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    soDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    tienLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    soNha = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    maTaiKhoan = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.maNhanVien);
                    table.ForeignKey(
                        name: "FK_NhanVien_TaiKhoan_maTaiKhoan",
                        column: x => x.maTaiKhoan,
                        principalTable: "TaiKhoan",
                        principalColumn: "maTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhieuDat",
                columns: table => new
                {
                    maPhieuDat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenNguoiDat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ngayTaoPhieu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trangThaiPhieuDat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    diaCHiGiaoHang = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    soDienThoaiGiaoHang = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ghiChu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    maKhachHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuDat", x => x.maPhieuDat);
                    table.ForeignKey(
                        name: "FK_PhieuDat_KhachHang_maKhachHang",
                        column: x => x.maKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "maKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuDat",
                columns: table => new
                {
                    maChiTietPhieuDat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    soLuongThue = table.Column<int>(type: "int", nullable: false),
                    giaThueNgay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ngayBatDauThue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngayKetThucThue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    maPhieuDat = table.Column<int>(type: "int", nullable: false),
                    maThietBi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuDat", x => x.maChiTietPhieuDat);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuDat_PhieuDat_maPhieuDat",
                        column: x => x.maPhieuDat,
                        principalTable: "PhieuDat",
                        principalColumn: "maPhieuDat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuDat_ThietBi_maThietBi",
                        column: x => x.maThietBi,
                        principalTable: "ThietBi",
                        principalColumn: "maThietBi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    maDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trangThaiDonHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngayGiaoHang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    maPhieuDat = table.Column<int>(type: "int", nullable: false),
                    maNhanVien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.maDonHang);
                    table.ForeignKey(
                        name: "FK_DonHang_NhanVien_maNhanVien",
                        column: x => x.maNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "maNhanVien");
                    table.ForeignKey(
                        name: "FK_DonHang_PhieuDat_maPhieuDat",
                        column: x => x.maPhieuDat,
                        principalTable: "PhieuDat",
                        principalColumn: "maPhieuDat",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuDat_maPhieuDat",
                table: "ChiTietPhieuDat",
                column: "maPhieuDat");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuDat_maThietBi",
                table: "ChiTietPhieuDat",
                column: "maThietBi");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_maNhanVien",
                table: "DonHang",
                column: "maNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_maPhieuDat",
                table: "DonHang",
                column: "maPhieuDat");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_maTaiKhoan",
                table: "GioHang",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_maTaiKhoan",
                table: "KhachHang",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_maTaiKhoan",
                table: "NhanVien",
                column: "maTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDat_maKhachHang",
                table: "PhieuDat",
                column: "maKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_maVaiTro",
                table: "TaiKhoan",
                column: "maVaiTro");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietPhieuDat");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "PhieuDat");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "VaiTro");
        }
    }
}
