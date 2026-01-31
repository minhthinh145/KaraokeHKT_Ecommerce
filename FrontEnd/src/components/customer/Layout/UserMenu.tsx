import React, { useState } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import type { RootState, AppDispatch } from "../../../redux/store";
import { logout } from "../../../redux/auth/authSlice";
import { useToast } from "../../../hooks/useToast";

export const UserMenu: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { showSuccess } = useToast();

  const { isAuthenticated, user } = useSelector(
    (state: RootState) => state.auth
  );
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  // 🔥 Logic đăng xuất
  const handleLogout = () => {
    dispatch(logout()); // Clear Redux state + localStorage
    setIsDropdownOpen(false); // Đóng dropdown
    showSuccess("Đăng xuất thành công!");
    navigate("/", { replace: true }); // Chuyển về trang chủ
  };

  // 🔥 Handle navigate và đóng dropdown
  const handleNavigation = (path: string) => {
    setIsDropdownOpen(false);
    navigate(path);
  };

  if (!isAuthenticated) {
    return (
      <div className="flex items-center space-x-4">
        <button
          onClick={() => navigate("/login")}
          className="text-black hover:text-indigo-600 font-medium"
        >
          Đăng nhập
        </button>
        <button
          onClick={() => navigate("/signup")}
          className="bg-indigo-700 text-white px-4 py-2 rounded-lg hover:bg-indigo-800 transition-colors"
        >
          Đăng ký
        </button>
      </div>
    );
  }

  return (
    <div className="relative">
      <button
        onClick={() => setIsDropdownOpen(!isDropdownOpen)}
        className="flex items-center gap-2 hover:bg-gray-50 p-2 rounded-lg transition-colors"
      >
        <div className="w-12 h-12 bg-gray-200 rounded-full flex items-center justify-center overflow-hidden">
          <div className="w-8 h-8 bg-black rounded-full" />
        </div>
        <span className="text-black text-xl font-medium font-['Space_Grotesk']">
          {user?.profile?.userName || "User"}
        </span>
      </button>

      {/* 🔥 Dropdown menu với logic hoàn chỉnh */}
      {isDropdownOpen && (
        <div className="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg border border-gray-200 z-50">
          <div className="py-2">
            <button
              onClick={() => handleNavigation("/customer/profile")}
              className="w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-50 transition-colors"
            >
              Thông tin cá nhân
            </button>
            <button
              onClick={() => handleNavigation("/customer/booking/history")}
              className="w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-50 transition-colors"
            >
              Lịch sử đặt phòng
            </button>
            <hr className="my-2" />
            <button
              onClick={handleLogout}
              className="w-full text-left px-4 py-2 text-red-600 hover:bg-gray-50 transition-colors"
            >
              Đăng xuất
            </button>
          </div>
        </div>
      )}

      {/* 🔥 Overlay để đóng dropdown khi click ngoài */}
      {isDropdownOpen && (
        <div
          className="fixed inset-0 z-40"
          onClick={() => setIsDropdownOpen(false)}
        />
      )}
    </div>
  );
};
