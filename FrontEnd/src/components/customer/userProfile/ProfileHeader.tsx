import React from "react";
import type { UserProfileDTO } from "../../../api/types/auth/UserProfileDTO";
import type { AuthUser } from "../../../types/auth";

interface ProfileHeaderProps {
  user: AuthUser | null;
  isEditing: boolean;
  onUpdateInfo: () => void;
  onCancel?: () => void;
  loading?: boolean;
  canSave?: boolean;
  activeTab: "profile" | "password";
  onTabChange: (tab: "profile" | "password") => void;
}

export const ProfileHeader: React.FC<ProfileHeaderProps> = ({
  user,
  isEditing,
  onUpdateInfo,
  onCancel,
  loading = false,
  canSave = true,
  activeTab,
}) => {
  // Chỉ hiển thị buttons khi đang ở tab profile
  const showProfileButtons = activeTab === "profile";
  console.log(user);
  return (
    <div
      className={`px-6 py-8 ${
        activeTab === "profile"
          ? "bg-gradient-to-r from-indigo-500 to-purple-600"
          : "bg-gradient-to-r from-red-500 to-pink-600"
      }`}
    >
      <div className="flex flex-col sm:flex-row items-center space-y-4 sm:space-y-0 sm:space-x-6">
        {/* Avatar */}
        <div className="w-24 h-24 bg-white rounded-full flex items-center justify-center text-2xl font-bold">
          <span
            className={
              activeTab === "profile" ? "text-indigo-600" : "text-red-600"
            }
          >
            {user?.profile?.userName?.charAt(0)?.toUpperCase() || "U"}
          </span>
        </div>

        {/* User Info */}
        <div className="text-center sm:text-left flex-1">
          <h1 className="text-2xl font-bold text-white">
            {activeTab === "profile"
              ? user?.profile?.userName || "Người dùng"
              : "Bảo mật tài khoản"}
          </h1>
          <p
            className={`mt-1 ${
              activeTab === "profile" ? "text-indigo-100" : "text-red-100"
            }`}
          >
            {activeTab === "profile"
              ? user?.profile?.email || ""
              : "Đổi mật khẩu để bảo vệ tài khoản của bạn"}
          </p>
        </div>

        {/* Action Buttons - Chỉ hiện khi ở tab profile */}
        {showProfileButtons && (
          <div className="flex space-x-3">
            {isEditing && onCancel && (
              <button
                onClick={onCancel}
                disabled={loading}
                className="px-4 py-2 bg-gray-500 hover:bg-gray-600 text-white rounded-lg font-semibold transition-all duration-200 disabled:opacity-50"
              >
                Hủy
              </button>
            )}

            <button
              onClick={onUpdateInfo}
              disabled={loading || (isEditing && !canSave)}
              className={`px-6 py-3 rounded-lg font-semibold transition-all duration-200 ${
                loading
                  ? "bg-gray-400 text-white cursor-not-allowed"
                  : isEditing
                  ? canSave
                    ? "bg-green-500 hover:bg-green-600 text-white"
                    : "bg-gray-400 text-white cursor-not-allowed"
                  : "bg-white text-indigo-600 hover:bg-gray-50"
              }`}
            >
              {loading
                ? "Đang cập nhật..."
                : isEditing
                ? "Lưu thay đổi"
                : "Cập nhật thông tin"}
            </button>
          </div>
        )}
      </div>
    </div>
  );
};
