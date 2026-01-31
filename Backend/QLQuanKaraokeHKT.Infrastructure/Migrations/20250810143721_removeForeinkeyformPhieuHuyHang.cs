using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class removeForeinkeyformPhieuHuyHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LichLamViec_PhieuHuyHang_PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec");

            migrationBuilder.DropIndex(
                name: "IX_LichLamViec_PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec");

            migrationBuilder.DropColumn(
                name: "PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichLamViec_PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec",
                column: "PhieuHuyHangMaPhieuHuyHang");

            migrationBuilder.AddForeignKey(
                name: "FK_LichLamViec_PhieuHuyHang_PhieuHuyHangMaPhieuHuyHang",
                table: "LichLamViec",
                column: "PhieuHuyHangMaPhieuHuyHang",
                principalTable: "PhieuHuyHang",
                principalColumn: "maPhieuHuyHang");
        }
    }
}
