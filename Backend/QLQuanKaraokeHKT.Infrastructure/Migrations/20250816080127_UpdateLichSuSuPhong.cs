using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLichSuSuPhong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaThuePhong",
                table: "LichSuSuDungPhong",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LichSuSuDungPhong_MaThuePhong",
                table: "LichSuSuDungPhong",
                column: "MaThuePhong");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuSuD__maThu__4B7734FF",
                table: "LichSuSuDungPhong",
                column: "MaThuePhong",
                principalTable: "ThuePhong",
                principalColumn: "maThuePhong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__LichSuSuD__maThu__4B7734FF",
                table: "LichSuSuDungPhong");

            migrationBuilder.DropIndex(
                name: "IX_LichSuSuDungPhong_MaThuePhong",
                table: "LichSuSuDungPhong");

            migrationBuilder.DropColumn(
                name: "MaThuePhong",
                table: "LichSuSuDungPhong");
        }
    }
}
