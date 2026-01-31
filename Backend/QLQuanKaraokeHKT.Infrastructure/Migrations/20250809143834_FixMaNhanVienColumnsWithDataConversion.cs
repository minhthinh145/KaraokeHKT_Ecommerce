using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLQuanKaraokeHKT.Migrations
{
    /// <inheritdoc />
    public partial class FixMaNhanVienColumnsWithDataConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Thêm cột mới kiểu Guid (nullable tạm thời)
            migrationBuilder.AddColumn<Guid?>(
                name: "MaNhanVien_New",
                table: "PhieuNhapHang",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "MaNhanVien_New",
                table: "PhieuKiemHang",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid?>(
                name: "MaNhanVien_New",
                table: "PhieuHuyHang",
                type: "uniqueidentifier",
                nullable: true);

            // 2. Convert dữ liệu (nếu có logic mapping từ int sang Guid)
            // VÍ DỤ: Nếu có bảng mapping hoặc logic convert
            migrationBuilder.Sql(@"
            UPDATE PhieuNhapHang 
            SET MaNhanVien_New = NEWID() 
            WHERE MaNhanVien IS NOT NULL;
            
            UPDATE PhieuKiemHang 
            SET MaNhanVien_New = NEWID() 
            WHERE MaNhanVien IS NOT NULL;
            
            UPDATE PhieuHuyHang 
            SET MaNhanVien_New = NEWID() 
            WHERE MaNhanVien IS NOT NULL;
        ");

            // 3. Drop cột cũ
            migrationBuilder.DropColumn(name: "MaNhanVien", table: "PhieuNhapHang");
            migrationBuilder.DropColumn(name: "MaNhanVien", table: "PhieuKiemHang");
            migrationBuilder.DropColumn(name: "MaNhanVien", table: "PhieuHuyHang");

            // 4. Rename cột mới thành tên cũ
            migrationBuilder.RenameColumn(
                name: "MaNhanVien_New",
                table: "PhieuNhapHang",
                newName: "MaNhanVien");

            migrationBuilder.RenameColumn(
                name: "MaNhanVien_New",
                table: "PhieuKiemHang",
                newName: "MaNhanVien");

            migrationBuilder.RenameColumn(
                name: "MaNhanVien_New",
                table: "PhieuHuyHang",
                newName: "MaNhanVien");

            // 5. Set NOT NULL
            migrationBuilder.AlterColumn<Guid>(
                name: "MaNhanVien",
                table: "PhieuNhapHang",
                nullable: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNhanVien",
                table: "PhieuKiemHang",
                nullable: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNhanVien",
                table: "PhieuHuyHang",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa hết dữ liệu để chắc chắn (nếu có)
            migrationBuilder.Sql("DELETE FROM PhieuNhapHang");
            migrationBuilder.Sql("DELETE FROM PhieuKiemHang");
            migrationBuilder.Sql("DELETE FROM PhieuHuyHang");

            // Đổi kiểu trực tiếp
            migrationBuilder.AlterColumn<Guid>(
                name: "MaNhanVien",
                table: "PhieuNhapHang",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNhanVien",
                table: "PhieuKiemHang",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNhanVien",
                table: "PhieuHuyHang",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
