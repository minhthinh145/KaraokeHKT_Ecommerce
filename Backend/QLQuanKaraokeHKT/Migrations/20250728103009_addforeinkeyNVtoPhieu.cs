using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class addforeinkeyNVtoPhieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaNhanVien",
                table: "PhieuNhapHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "NhanVienMaNv",
                table: "PhieuNhapHang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MaNhanVien",
                table: "PhieuKiemHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "NhanVienMaNv",
                table: "PhieuKiemHang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MaNhanVien",
                table: "PhieuHuyHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "NhanVienMaNv",
                table: "PhieuHuyHang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhapHang_NhanVienMaNv",
                table: "PhieuNhapHang",
                column: "NhanVienMaNv");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuKiemHang_NhanVienMaNv",
                table: "PhieuKiemHang",
                column: "NhanVienMaNv");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuHuyHang_NhanVienMaNv",
                table: "PhieuHuyHang",
                column: "NhanVienMaNv");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuHuyHang_NhanVien_NhanVienMaNv",
                table: "PhieuHuyHang",
                column: "NhanVienMaNv",
                principalTable: "NhanVien",
                principalColumn: "maNV",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuKiemHang_NhanVien_NhanVienMaNv",
                table: "PhieuKiemHang",
                column: "NhanVienMaNv",
                principalTable: "NhanVien",
                principalColumn: "maNV",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuNhapHang_NhanVien_NhanVienMaNv",
                table: "PhieuNhapHang",
                column: "NhanVienMaNv",
                principalTable: "NhanVien",
                principalColumn: "maNV",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuHuyHang_NhanVien_NhanVienMaNv",
                table: "PhieuHuyHang");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuKiemHang_NhanVien_NhanVienMaNv",
                table: "PhieuKiemHang");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuNhapHang_NhanVien_NhanVienMaNv",
                table: "PhieuNhapHang");

            migrationBuilder.DropIndex(
                name: "IX_PhieuNhapHang_NhanVienMaNv",
                table: "PhieuNhapHang");

            migrationBuilder.DropIndex(
                name: "IX_PhieuKiemHang_NhanVienMaNv",
                table: "PhieuKiemHang");

            migrationBuilder.DropIndex(
                name: "IX_PhieuHuyHang_NhanVienMaNv",
                table: "PhieuHuyHang");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "PhieuNhapHang");

            migrationBuilder.DropColumn(
                name: "NhanVienMaNv",
                table: "PhieuNhapHang");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "PhieuKiemHang");

            migrationBuilder.DropColumn(
                name: "NhanVienMaNv",
                table: "PhieuKiemHang");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "PhieuHuyHang");

            migrationBuilder.DropColumn(
                name: "NhanVienMaNv",
                table: "PhieuHuyHang");
        }
    }
}
