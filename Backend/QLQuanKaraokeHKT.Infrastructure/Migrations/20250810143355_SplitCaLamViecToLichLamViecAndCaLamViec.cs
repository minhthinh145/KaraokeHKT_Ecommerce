using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class SplitCaLamViecToLichLamViecAndCaLamViec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__CaLamViec__maNha__1AD3FDA4",
                table: "CaLamViec");

            migrationBuilder.DropPrimaryKey(
                name: "PK__CaLamVie__84019BCC8B786A53",
                table: "CaLamViec");

            migrationBuilder.DropIndex(
                name: "IX_CaLamViec_NgayLam",
                table: "CaLamViec");

            migrationBuilder.DropIndex(
                name: "IX_CaLamViec_NhanVien",
                table: "CaLamViec");

            migrationBuilder.DropColumn(
                name: "maNhanVien",
                table: "CaLamViec");

            migrationBuilder.DropColumn(
                name: "ngayLamViec",
                table: "CaLamViec");

            migrationBuilder.RenameColumn(
                name: "maCaLamViec",
                table: "CaLamViec",
                newName: "maCa");

            migrationBuilder.AddColumn<string>(
                name: "tenCa",
                table: "CaLamViec",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaLamViec",
                table: "CaLamViec",
                column: "maCa");

            migrationBuilder.CreateTable(
                name: "LichLamViec",
                columns: table => new
                {
                    maLichLamViec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ngayLamViec = table.Column<DateOnly>(type: "date", nullable: false),
                    maNhanVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maCa = table.Column<int>(type: "int", nullable: false),
                    PhieuHuyHangMaPhieuHuyHang = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichLamViec", x => x.maLichLamViec);
                    table.ForeignKey(
                        name: "FK_LichLamViec_CaLamViec_maCa",
                        column: x => x.maCa,
                        principalTable: "CaLamViec",
                        principalColumn: "maCa");
                    table.ForeignKey(
                        name: "FK_LichLamViec_NhanVien_maNhanVien",
                        column: x => x.maNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "maNV");
                    table.ForeignKey(
                        name: "FK_LichLamViec_PhieuHuyHang_PhieuHuyHangMaPhieuHuyHang",
                        column: x => x.PhieuHuyHangMaPhieuHuyHang,
                        principalTable: "PhieuHuyHang",
                        principalColumn: "maPhieuHuyHang");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_maCa",
                table: "LichLamViec",
                column: "maCa");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_maNhanVien",
                table: "LichLamViec",
                column: "maNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec",
                column: "PhieuHuyHangMaPhieuHuyHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichLamViec");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaLamViec",
                table: "CaLamViec");

            migrationBuilder.DropColumn(
                name: "tenCa",
                table: "CaLamViec");

            migrationBuilder.RenameColumn(
                name: "maCa",
                table: "CaLamViec",
                newName: "maCaLamViec");

            migrationBuilder.AddColumn<Guid>(
                name: "maNhanVien",
                table: "CaLamViec",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateOnly>(
                name: "ngayLamViec",
                table: "CaLamViec",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK__CaLamVie__84019BCC8B786A53",
                table: "CaLamViec",
                column: "maCaLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_CaLamViec_NgayLam",
                table: "CaLamViec",
                column: "ngayLamViec");

            migrationBuilder.CreateIndex(
                name: "IX_CaLamViec_NhanVien",
                table: "CaLamViec",
                column: "maNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK__CaLamViec__maNha__1AD3FDA4",
                table: "CaLamViec",
                column: "maNhanVien",
                principalTable: "NhanVien",
                principalColumn: "maNV");
        }
    }
}
