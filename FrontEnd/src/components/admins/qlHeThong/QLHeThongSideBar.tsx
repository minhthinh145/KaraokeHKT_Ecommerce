import React from "react";
import { UsersIcon } from "@heroicons/react/24/outline";

type TabType = "nhan-vien" | "khach-hang" | "quan-ly";

interface QLHeThongSidebarProps {
  isCollapsed: boolean;
  activeTab: TabType;
  onTabChange: (tab: TabType) => void;
}

export const QLHeThongSidebar: React.FC<QLHeThongSidebarProps> = ({
  isCollapsed,
  activeTab,
  onTabChange,
}) => {
  const MenuItem: React.FC<{
    id: TabType;
    title: string;
    icon: React.ComponentType<{ className?: string }>;
  }> = ({ id, title, icon: Icon }) => {
    const isActive = activeTab === id;

    return (
      <button
        onClick={() => onTabChange(id)}
        className={`w-full min-h-[44px] px-4 py-2 rounded-lg flex items-center gap-4 transition-colors ${
          isActive
            ? "bg-neutral-100 text-black"
            : "bg-white text-black hover:bg-gray-50"
        }`}
      >
        {Icon && <Icon className="w-6 h-6 flex-shrink-0" />}
        {!isCollapsed && (
          <span className="text-lg font-medium font-['Space_Grotesk'] break-words leading-tight">
            {title}
          </span>
        )}
      </button>
    );
  };

  return (
    <div className="p-4 space-y-6">
      {/* 🔥 Staff Account Section */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="px-4 py-2">
            <h3 className="text-lg font-bold font-['Space_Grotesk'] text-black leading-tight">
              Tài khoản nhân viên
            </h3>
          </div>

          <div className="space-y-1">
            <MenuItem id="nhan-vien" title="Nhân viên" icon={UsersIcon} />
          </div>
          <div className="space-y-1">
            <MenuItem id="quan-ly" title="Quản lý hệ thống" icon={UsersIcon} />
          </div>
          <div className="h-4"></div>
        </div>
      )}

      {/* 🔥 Customer Account Section */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="px-4 py-2">
            <h3 className="text-lg font-bold font-['Space_Grotesk'] text-black leading-tight">
              Tài khoản khách hàng
            </h3>
          </div>

          <div className="space-y-1">
            <MenuItem id="khach-hang" title="Khách hàng" icon={UsersIcon} />
          </div>

          <div className="h-4"></div>
        </div>
      )}

      {/* 🔥 Collapsed State - Larger icons */}
      {isCollapsed && (
        <div className="space-y-3">
          <MenuItem id="nhan-vien" title="Nhân viên kho" icon={UsersIcon} />
          <MenuItem id="khach-hang" title="Khách hàng" icon={UsersIcon} />
        </div>
      )}
    </div>
  );
};
