using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChoThueThietBiXD.Migrations
{
    public partial class updateKhachhangNhanvien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietBi_Hang_maHang",
                table: "ThietBi");

            migrationBuilder.DropColumn(
                name: "soNha",
                table: "NhanVien");

            migrationBuilder.DropColumn(
                name: "soNha",
                table: "KhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBi_Hang_maHang",
                table: "ThietBi",
                column: "maHang",
                principalTable: "Hang",
                principalColumn: "maHang",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietBi_Hang_maHang",
                table: "ThietBi");

            migrationBuilder.AddColumn<string>(
                name: "soNha",
                table: "NhanVien",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "soNha",
                table: "KhachHang",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBi_Hang_maHang",
                table: "ThietBi",
                column: "maHang",
                principalTable: "Hang",
                principalColumn: "maHang",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
