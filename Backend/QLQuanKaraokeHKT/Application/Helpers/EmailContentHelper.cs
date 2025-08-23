namespace QLQuanKaraokeHKT.Application.Helpers
{
    public static class EmailContentHelper
    {
        public static string CreateWelcomeEmployeeEmail(string hoTen, string email, string password)
        {
            return $"Chào nhân viên {hoTen},\n\n" +
                   "Tài khoản của bạn đã được tạo thành công. Vui lòng đăng nhập với thông tin sau:\n" +
                   $"Email: {email}\n" +
                   $"Mật khẩu: {password}";
        }
    }
}