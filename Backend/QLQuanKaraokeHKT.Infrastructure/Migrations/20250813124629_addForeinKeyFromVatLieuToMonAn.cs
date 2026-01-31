using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class addForeinKeyFromVatLieuToMonAn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaVatLieu",
                table: "MonAn",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonAn_MaVatLieu",
                table: "MonAn",
                column: "MaVatLieu",
                unique: true,
                filter: "[MaVatLieu] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MonAn_VatLieu_MaVatLieu",
                table: "MonAn",
                column: "MaVatLieu",
                principalTable: "VatLieu",
                principalColumn: "maVatLieu",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonAn_VatLieu_MaVatLieu",
                table: "MonAn");

            migrationBuilder.DropIndex(
                name: "IX_MonAn_MaVatLieu",
                table: "MonAn");

            migrationBuilder.DropColumn(
                name: "MaVatLieu",
                table: "MonAn");
        }
    }
}
