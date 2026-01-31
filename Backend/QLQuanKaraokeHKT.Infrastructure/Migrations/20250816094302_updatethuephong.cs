using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class updatethuephong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "maHoaDon",
                table: "ThuePhong",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThuePhong_HoaDon",
                table: "ThuePhong",
                column: "maHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK__ThuePhong__maHoa__45BE5BA9",
                table: "ThuePhong",
                column: "maHoaDon",
                principalTable: "HoaDonDichVu",
                principalColumn: "maHoaDon",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ThuePhong__maHoa__45BE5BA9",
                table: "ThuePhong");

            migrationBuilder.DropIndex(
                name: "IX_ThuePhong_HoaDon",
                table: "ThuePhong");

            migrationBuilder.DropColumn(
                name: "maHoaDon",
                table: "ThuePhong");
        }
    }
}
