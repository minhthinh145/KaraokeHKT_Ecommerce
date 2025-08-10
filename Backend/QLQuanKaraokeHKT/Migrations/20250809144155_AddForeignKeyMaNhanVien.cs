using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyMaNhanVien : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
          name: "IX_PhieuNhapHang_MaNhanVien",
          table: "PhieuNhapHang",
          column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuKiemHang_MaNhanVien",
                table: "PhieuKiemHang",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuHuyHang_MaNhanVien",
                table: "PhieuHuyHang",
                column: "MaNhanVien");

            // Tạo foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapHang_NhanVien_MaNhanVien",
                table: "PhieuNhapHang",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNv",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuKiemHang_NhanVien_MaNhanVien",
                table: "PhieuKiemHang",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNv",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuHuyHang_NhanVien_MaNhanVien",
                table: "PhieuHuyHang",
                column: "MaNhanVien",
                principalTable: "NhanVien",
                principalColumn: "MaNv",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHang_NhanVien_MaNhanVien",
                table: "PhieuNhapHang");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuKiemHang_NhanVien_MaNhanVien",
                table: "PhieuKiemHang");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuHuyHang_NhanVien_MaNhanVien",
                table: "PhieuHuyHang");

            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapHang_MaNhanVien",
                table: "PhieuNhapHang");

            migrationBuilder.DropIndex(
                name: "IX_PhieuKiemHang_MaNhanVien",
                table: "PhieuKiemHang");

            migrationBuilder.DropIndex(
                name: "IX_PhieuHuyHang_MaNhanVien",
                table: "PhieuHuyHang");
        }
    }
}
