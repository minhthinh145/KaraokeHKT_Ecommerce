import type { RootState } from "../store";
import { ApplicationRole } from "../../api/types/admins/QLHeThongtypes";
import { getDefaultRoute } from "../../constants/auth";

// Basic selectors
export const selectAuth = (state: RootState) => state.auth;
export const selectUser = (state: RootState) => state.auth.user;
export const selectIsAuthenticated = (state: RootState) =>
  state.auth.isAuthenticated;
export const selectAuthLoading = (state: RootState) => state.auth.loading;
export const selectAuthError = (state: RootState) => state.auth.error;
export const selectAccessToken = (state: RootState) => state.auth.accessToken;
export const selectRefreshToken = (state: RootState) => state.auth.refreshToken;

// User field selectors (theo UserProfileDTO thực tế)
export const selectUserName = (state: RootState) =>
  state.auth.user?.profile?.userName || "";
export const selectUserEmail = (state: RootState) =>
  state.auth.user?.profile?.email || "";
export const selectUserPhone = (state: RootState) =>
  state.auth.user?.profile?.phone || "";
export const selectUserBirthDate = (state: RootState) =>
  state.auth.user?.profile?.birthDate || "";
export const selectUserAccountType = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan || "";
export const selectFormattedBirthDate = (state: RootState) =>
  state.auth.user?.profile?.birthDateFormatted || "";
export const selectUserRole = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan;
// Computed selectors
export const selectUserDisplayName = (state: RootState) => {
  const user = state.auth.user;
  return user?.profile?.userName || "User";
};

export const selectUserInitials = (state: RootState) => {
  const name = state.auth.user?.profile?.userName || "U";
  return name
    .split(" ")
    .map((word: string) => word.charAt(0))
    .join("")
    .substring(0, 2)
    .toUpperCase();
};

export const selectUserFullDisplayName = (state: RootState) => {
  const user = state.auth.user;
  return user?.profile?.userName || "Người dùng";
};

export const selectFormattedBirthDateDisplay = (state: RootState) => {
  const birthDate = state.auth.user?.profile?.birthDate;
  if (!birthDate) return "";
  try {
    return new Date(birthDate).toLocaleDateString("vi-VN");
  } catch {
    return birthDate;
  }
};

export const selectUserAge = (state: RootState) => {
  const birthDate = state.auth.user?.profile?.birthDate;
  if (!birthDate) return null;
  try {
    const today = new Date();
    const birth = new Date(birthDate);
    let age = today.getFullYear() - birth.getFullYear();
    const monthDiff = today.getMonth() - birth.getMonth();
    if (
      monthDiff < 0 ||
      (monthDiff === 0 && today.getDate() < birth.getDate())
    ) {
      age--;
    }
    return age;
  } catch {
    return null;
  }
};

// Profile completion selectors
export const selectIsProfileComplete = (state: RootState) => {
  const profile = state.auth.user?.profile;
  return !!(
    profile?.userName &&
    profile?.phone &&
    profile?.birthDate &&
    profile?.email
  );
};

export const selectMissingProfileFields = (state: RootState) => {
  const profile = state.auth.user?.profile;
  const missing: string[] = [];
  if (!profile?.userName) missing.push("Tên người dùng");
  if (!profile?.phone) missing.push("Số điện thoại");
  if (!profile?.birthDate) missing.push("Ngày sinh");
  if (!profile?.email) missing.push("Email");
  return missing;
};

export const selectProfileCompletionPercentage = (state: RootState) => {
  const profile = state.auth.user?.profile;
  if (!profile) return 0;
  const fields = [
    profile.userName,
    profile.email,
    profile.phone,
    profile.birthDate,
  ];
  const filledFields = fields.filter(
    (field) => field && field.trim() !== ""
  ).length;
  return Math.round((filledFields / fields.length) * 100);
};

// Status selectors
export const selectIsLoading = (state: RootState) => state.auth.loading;
export const selectHasError = (state: RootState) => !!state.auth.error;
export const selectIsIdle = (state: RootState) =>
  !state.auth.loading && !state.auth.error;

// Conditional selectors
export const selectCanUpdateProfile = (state: RootState) => {
  return state.auth.isAuthenticated && !state.auth.loading;
};

export const selectShouldShowOnboarding = (state: RootState) => {
  const profile = state.auth.user?.profile;
  return state.auth.isAuthenticated && (!profile?.userName || !profile?.phone);
};

// 🔥 SỬA LẠI: Account type selectors - dùng đúng ApplicationRole values
export const selectIsQuanTriHeThong = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.QuanTriHeThong;

export const selectIsQuanLyNhanSu = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.QuanLyNhanSu;

export const selectIsQuanLyKho = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.QuanLyKho;

export const selectIsQuanLyPhongHat = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.QuanLyPhongHat;

export const selectIsNhanVienKho = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.NhanVienKho;

export const selectIsNhanVienPhucVu = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.NhanVienPhucVu;

export const selectIsNhanVienTiepTan = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.NhanVienTiepTan;

// Role group selectors
export const selectIsAdmin = (state: RootState) =>
  state.auth.user?.loaiTaiKhoan === ApplicationRole.QuanTriHeThong;

export const selectIsManager = (state: RootState) => {
  const role = state.auth.user?.loaiTaiKhoan;
  return [
    ApplicationRole.QuanLyNhanSu,
    ApplicationRole.QuanLyKho,
    ApplicationRole.QuanLyPhongHat,
  ].includes(role as any);
};

export const selectIsStaff = (state: RootState) => {
  const role = state.auth.user?.loaiTaiKhoan;
  return [
    ApplicationRole.NhanVienKho,
    ApplicationRole.NhanVienPhucVu,
    ApplicationRole.NhanVienTiepTan,
  ].includes(role as any);
};

// Validation selectors
export const selectIsValidEmail = (state: RootState) => {
  const email = state.auth.user?.profile?.email;
  if (!email) return false;
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

export const selectIsValidPhone = (state: RootState) => {
  const phone = state.auth.user?.profile?.phone;
  if (!phone) return false;
  const phoneRegex = /^[0-9]{10,11}$/;
  return phoneRegex.test(phone.replace(/\s/g, ""));
};

export const selectIsValidBirthDate = (state: RootState) => {
  const birthDate = state.auth.user?.profile?.birthDate;
  if (!birthDate) return false;
  try {
    const date = new Date(birthDate);
    const today = new Date();
    return date <= today && date.getFullYear() > 1900;
  } catch {
    return false;
  }
};

// Formatted data selectors
export const selectFormattedPhone = (state: RootState) => {
  const phone = state.auth.user?.profile?.phone;
  if (!phone) return "";
  const cleaned = phone.replace(/\D/g, "");
  if (cleaned.length === 10) {
    return `${cleaned.slice(0, 4)} ${cleaned.slice(4, 7)} ${cleaned.slice(7)}`;
  }
  return phone;
};

// 🔥 SỬA LẠI: Account type display - dùng đúng ApplicationRole
export const selectAccountTypeDisplay = (state: RootState) => {
  const accountType = state.auth.user?.loaiTaiKhoan;
  switch (accountType) {
    case ApplicationRole.QuanTriHeThong:
      return "Quản trị hệ thống";
    case ApplicationRole.QuanLyNhanSu:
      return "Quản lý nhân sự";
    case ApplicationRole.QuanLyKho:
      return "Quản lý kho";
    case ApplicationRole.QuanLyPhongHat:
      return "Quản lý phòng hát";
    case ApplicationRole.NhanVienKho:
      return "Nhân viên kho";
    case ApplicationRole.NhanVienPhucVu:
      return "Nhân viên phục vụ";
    case ApplicationRole.NhanVienTiepTan:
      return "Nhân viên tiếp tân";
    default:
      return "Người dùng";
  }
};

export const selectUserDefaultRoute = (state: RootState) => {
  const role = state.auth.user?.loaiTaiKhoan;
  return getDefaultRoute(role);
};
