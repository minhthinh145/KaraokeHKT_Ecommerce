using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class AddDaNghiViecAndNgungCungCap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NgungCungCap",
                table: "VatLieu",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgungCungCap",
                table: "VatLieu");
        }
    }
}
