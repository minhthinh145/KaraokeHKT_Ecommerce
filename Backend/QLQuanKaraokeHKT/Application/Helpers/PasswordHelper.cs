using QLQuanKaraokeHKT.Core.Common;

namespace QLQuanKaraokeHKT.Application.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateAdminPassword(string roleCode)
        {
            string prefix = roleCode switch
            {
                ApplicationRole.QuanLyKho => "qlKho",
                ApplicationRole.QuanTriHeThong => "qtHeThong",
                ApplicationRole.QuanLyNhanSu => "qlNhanSu",
                ApplicationRole.QuanLyPhongHat => "qlPhongHat",
                _ => "admin"
            };
            return $"{prefix}@Admin123";
        }

        public static string FormatPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return password;

            int index = 0;
            while (index < password.Length && !char.IsLetter(password[index]))
            {
                index++;
            }

            if (index < password.Length)
            {
                return password.Substring(0, index)
                     + char.ToUpper(password[index])
                     + password[(index + 1)..];
            }

            return password;
        }
    }
}
