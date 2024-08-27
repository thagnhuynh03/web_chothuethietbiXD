using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChoThueThietBiXD.Migrations
{
    public partial class ThietbiDanhmucHinhanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucThietBi",
                columns: table => new
                {
                    maDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucThietBi", x => x.maDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "ThietBi",
                columns: table => new
                {
                    maThietBi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenThietBi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    giaThue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    soLuongCon = table.Column<int>(type: "int", nullable: false),
                    maDanhMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietBi", x => x.maThietBi);
                    table.ForeignKey(
                        name: "FK_ThietBi_DanhMucThietBi_maDanhMuc",
                        column: x => x.maDanhMuc,
                        principalTable: "DanhMucThietBi",
                        principalColumn: "maDanhMuc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhThietBi",
                columns: table => new
                {
                    maHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    maThietBi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhThietBi", x => x.maHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhThietBi_ThietBi_maThietBi",
                        column: x => x.maThietBi,
                        principalTable: "ThietBi",
                        principalColumn: "maThietBi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhThietBi_maThietBi",
                table: "HinhAnhThietBi",
                column: "maThietBi");

            migrationBuilder.CreateIndex(
                name: "IX_ThietBi_maDanhMuc",
                table: "ThietBi",
                column: "maDanhMuc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhAnhThietBi");

            migrationBuilder.DropTable(
                name: "ThietBi");

            migrationBuilder.DropTable(
                name: "DanhMucThietBi");
        }
    }
}
