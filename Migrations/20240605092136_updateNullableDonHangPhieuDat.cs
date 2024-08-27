using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebChoThueThietBiXD.Migrations
{
    public partial class updateNullableDonHangPhieuDat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ghiChu",
                table: "PhieuDat",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
            migrationBuilder.AlterColumn<DateTime>(
            name: "ngayThanhToan",
            table: "DonHang",
            type: "datetime2",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngayGiaoHang",
                table: "DonHang",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ghiChu",
                table: "PhieuDat",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
            migrationBuilder.AlterColumn<DateTime>(
            name: "ngayThanhToan",
            table: "DonHang",
            type: "datetime2",
            nullable: false,
            defaultValue: DateTime.Now,
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldNullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngayGiaoHang",
                table: "DonHang",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: false);
        }
    }
}
