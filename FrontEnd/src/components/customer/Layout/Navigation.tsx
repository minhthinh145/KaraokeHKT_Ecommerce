import React from "react";
import { useNavigate, useLocation } from "react-router-dom";

export const Navigation: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const navigationItems = [
    { label: "Trang chủ", path: "/" },
    { label: "Đặt phòng", path: "/customer/booking" },
    { label: "Dịch vụ", path: "/customer/services" },
    { label: "Liên hệ", path: "/customer/contact" },
  ];

  const handleNavigation = (path: string) => {
    navigate(path);
  };

  return (
    <nav className="hidden md:flex space-x-8">
      {navigationItems.map((item) => (
        <button
          key={item.path}
          onClick={() => handleNavigation(item.path)}
          className={`px-3 py-2 text-lg  font-bold transition-colors ${
            location.pathname === item.path
              ? "text-indigo-600 border-b-2 border-indigo-600"
              : "text-gray-700 hover:text-indigo-600"
          }`}
        >
          {item.label}
        </button>
      ))}
    </nav>
  );
};
