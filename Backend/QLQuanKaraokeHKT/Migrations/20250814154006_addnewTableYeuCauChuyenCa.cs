using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class addnewTableYeuCauChuyenCa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YeuCauChuyenCa",
                columns: table => new
                {
                    maYeuCau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maLichLamViecGoc = table.Column<int>(type: "int", nullable: false),
                    ngayLamViecMoi = table.Column<DateOnly>(type: "date", nullable: false),
                    maCaMoi = table.Column<int>(type: "int", nullable: false),
                    lyDoChuyenCa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    daPheDuyet = table.Column<bool>(type: "bit", nullable: false),
                    ketQuaPheDuyet = table.Column<bool>(type: "bit", nullable: true),
                    ghiChuPheDuyet = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ngayTaoYeuCau = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ngayPheDuyet = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauChuyenCa", x => x.maYeuCau);
                    table.ForeignKey(
                        name: "FK_YeuCauChuyenCa_CaLamViec",
                        column: x => x.maCaMoi,
                        principalTable: "CaLamViec",
                        principalColumn: "maCa");
                    table.ForeignKey(
                        name: "FK_YeuCauChuyenCa_LichLamViec",
                        column: x => x.maLichLamViecGoc,
                        principalTable: "LichLamViec",
                        principalColumn: "maLichLamViec");
                });

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauChuyenCa_maCaMoi",
                table: "YeuCauChuyenCa",
                column: "maCaMoi");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauChuyenCa_maLichLamViecGoc",
                table: "YeuCauChuyenCa",
                column: "maLichLamViecGoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YeuCauChuyenCa");
        }
    }
}
