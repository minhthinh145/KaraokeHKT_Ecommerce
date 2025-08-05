import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import type { RootState } from "../../../redux/store";

const navigationItems = [
  { label: "Trang chủ", href: "/" },
  { label: "Đặt phòng", href: "/booking" },
  { label: "Dịch vụ", href: "/services" },
  { label: "Liên hệ", href: "/contact" },
];

export const Navigation: React.FC = () => {
  const navigate = useNavigate();
  const { isAuthenticated } = useSelector((state: RootState) => state.auth);

  const handleNavClick = (href: string, e: React.MouseEvent) => {
    if (href === "/") {
      // Trang chủ thì cho vào tự do
      return;
    }

    // Các trang khác cần login
    if (!isAuthenticated) {
      e.preventDefault();
      navigate("/login");
    }
  };

  return (
    <nav className="flex items-center space-x-8">
      {navigationItems.map((item) => (
        <Link
          key={item.href}
          to={item.href}
          onClick={(e) => handleNavClick(item.href, e)}
          className="text-2xl font-medium font-['Space_Grotesk'] text-black hover:text-indigo-600 transition-colors"
        >
          {item.label}
        </Link>
      ))}
    </nav>
  );
};
