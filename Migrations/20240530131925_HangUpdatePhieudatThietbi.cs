using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChoThueThietBiXD.Migrations
{
    public partial class HangUpdatePhieudatThietbi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tenNguoiDat",
                table: "PhieuDat");

            migrationBuilder.AddColumn<int>(
                name: "maHang",
                table: "ThietBi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Hang",
                columns: table => new
                {
                    maHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hang", x => x.maHang);
                });

            // Seed data into Hang table
            migrationBuilder.InsertData(
                table: "Hang",
                columns: new[] { "maHang", "tenHang" },
                values: new object[,]
                {
                    { 1, "Hang 1" },
                    { 2, "Hang 2" }
                });

            // Update existing records in ThietBi to use valid maHang values
            migrationBuilder.Sql("UPDATE ThietBi SET maHang = 1 WHERE maHang = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBi_maHang",
                table: "ThietBi",
                column: "maHang");

            migrationBuilder.AddForeignKey(
                name: "FK_ThietBi_Hang_maHang",
                table: "ThietBi",
                column: "maHang",
                principalTable: "Hang",
                principalColumn: "maHang",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietBi_Hang_maHang",
                table: "ThietBi");

            migrationBuilder.DropTable(
                name: "Hang");

            migrationBuilder.DropIndex(
                name: "IX_ThietBi_maHang",
                table: "ThietBi");

            migrationBuilder.DropColumn(
                name: "maHang",
                table: "ThietBi");

            migrationBuilder.AddColumn<string>(
                name: "tenNguoiDat",
                table: "PhieuDat",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
