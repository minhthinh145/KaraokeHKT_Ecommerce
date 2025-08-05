import type { RootState } from "../store";

// 🔥 Basic selectors
export const selectAuth = (state: RootState) => state.auth;
export const selectUser = (state: RootState) => state.auth.user;
export const selectIsAuthenticated = (state: RootState) =>
  state.auth.isAuthenticated;
export const selectAuthLoading = (state: RootState) => state.auth.loading;
export const selectAuthError = (state: RootState) => state.auth.error;
export const selectAccessToken = (state: RootState) => state.auth.accessToken;
export const selectRefreshToken = (state: RootState) => state.auth.refreshToken;

// 🔥 User field selectors (theo UserProfileDTO thực tế)
export const selectUserName = (state: RootState) => {
  return state.auth.user?.userName || "";
};

export const selectUserEmail = (state: RootState) => {
  return state.auth.user?.email || "";
};

export const selectUserPhone = (state: RootState) => {
  return state.auth.user?.phone || "";
};

export const selectUserBirthDate = (state: RootState) => {
  return state.auth.user?.birthDate || "";
};

export const selectUserAccountType = (state: RootState) => {
  return state.auth.user?.loaiTaiKhoan || "customer";
};

export const selectFormattedBirthDate = (state: RootState) => {
  return state.auth.user?.birthDateFormatted || "";
};

// 🔥 Computed selectors
export const selectUserDisplayName = (state: RootState) => {
  const user = state.auth.user;
  return user?.userName || "User";
};

export const selectUserInitials = (state: RootState) => {
  const user = state.auth.user;
  const name = user?.userName || "U";
  return name
    .split(" ")
    .map((word: string) => word.charAt(0))
    .join("")
    .substring(0, 2)
    .toUpperCase();
};

export const selectUserFullDisplayName = (state: RootState) => {
  const user = state.auth.user;
  return user?.userName || "Người dùng";
};

export const selectFormattedBirthDateDisplay = (state: RootState) => {
  const birthDate = state.auth.user?.birthDate;
  if (!birthDate) return "";

  try {
    return new Date(birthDate).toLocaleDateString("vi-VN");
  } catch {
    return birthDate;
  }
};

export const selectUserAge = (state: RootState) => {
  const birthDate = state.auth.user?.birthDate;
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

// 🔥 Profile completion selectors (chỉ với fields có sẵn)
export const selectIsProfileComplete = (state: RootState) => {
  const user = state.auth.user;
  return !!(user?.userName && user?.phone && user?.birthDate && user?.email);
};

export const selectMissingProfileFields = (state: RootState) => {
  const user = state.auth.user;
  const missing: string[] = [];

  if (!user?.userName) missing.push("Tên người dùng");
  if (!user?.phone) missing.push("Số điện thoại");
  if (!user?.birthDate) missing.push("Ngày sinh");
  if (!user?.email) missing.push("Email");

  return missing;
};

export const selectProfileCompletionPercentage = (state: RootState) => {
  const user = state.auth.user;
  if (!user) return 0;

  const fields = [user.userName, user.email, user.phone, user.birthDate];

  const filledFields = fields.filter(
    (field) => field && field.trim() !== ""
  ).length;
  return Math.round((filledFields / fields.length) * 100);
};

// 🔥 Status selectors
export const selectIsLoading = (state: RootState) => state.auth.loading;
export const selectHasError = (state: RootState) => !!state.auth.error;
export const selectIsIdle = (state: RootState) =>
  !state.auth.loading && !state.auth.error;

// 🔥 Conditional selectors
export const selectCanUpdateProfile = (state: RootState) => {
  return state.auth.isAuthenticated && !state.auth.loading;
};

export const selectShouldShowOnboarding = (state: RootState) => {
  const user = state.auth.user;
  return state.auth.isAuthenticated && (!user?.userName || !user?.phone);
};

// 🔥 Account type selectors
export const selectIsCustomer = (state: RootState) => {
  return state.auth.user?.loaiTaiKhoan === "customer";
};

export const selectIsAdmin = (state: RootState) => {
  return state.auth.user?.loaiTaiKhoan === "admin";
};

export const selectIsStaff = (state: RootState) => {
  return state.auth.user?.loaiTaiKhoan === "staff";
};

// 🔥 Validation selectors
export const selectIsValidEmail = (state: RootState) => {
  const email = state.auth.user?.email;
  if (!email) return false;

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

export const selectIsValidPhone = (state: RootState) => {
  const phone = state.auth.user?.phone;
  if (!phone) return false;

  const phoneRegex = /^[0-9]{10,11}$/;
  return phoneRegex.test(phone.replace(/\s/g, ""));
};

export const selectIsValidBirthDate = (state: RootState) => {
  const birthDate = state.auth.user?.birthDate;
  if (!birthDate) return false;

  try {
    const date = new Date(birthDate);
    const today = new Date();
    return date <= today && date.getFullYear() > 1900;
  } catch {
    return false;
  }
};

// 🔥 Formatted data selectors
export const selectFormattedPhone = (state: RootState) => {
  const phone = state.auth.user?.phone;
  if (!phone) return "";

  // Format: 0123 456 789
  const cleaned = phone.replace(/\D/g, "");
  if (cleaned.length === 10) {
    return `${cleaned.slice(0, 4)} ${cleaned.slice(4, 7)} ${cleaned.slice(7)}`;
  }
  return phone;
};

export const selectAccountTypeDisplay = (state: RootState) => {
  const accountType = state.auth.user?.loaiTaiKhoan;
  switch (accountType) {
    case "customer":
      return "Khách hàng";
    case "admin":
      return "Quản trị viên";
    case "staff":
      return "Nhân viên";
    default:
      return "Khách hàng";
  }
};
