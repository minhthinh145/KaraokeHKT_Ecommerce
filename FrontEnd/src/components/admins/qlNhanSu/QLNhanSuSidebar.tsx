import React from "react";
import {
  CalendarDaysIcon,
  CurrencyDollarIcon,
  UsersIcon,
  ClockIcon,
  ArrowPathIcon,
  ChartBarIcon,
  CogIcon,
  IdentificationIcon,
} from "@heroicons/react/24/outline";

type TabType =
  | "sap-xep-lich"
  | "duyet-yeu-cau"
  | "thong-ke-luong"
  | "dieu-chinh-luong"
  | "thong-tin-nhan-vien";

interface QLNhanSuSidebarProps {
  isCollapsed: boolean;
  activeTab: TabType;
  onTabChange: (tab: TabType) => void;
}

export const QLNhanSuSidebar: React.FC<QLNhanSuSidebarProps> = ({
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
        <Icon className="w-6 h-6 flex-shrink-0" />
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
      {/* 🔥 Quản lý ca làm */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="px-4 py-2">
            <h3 className="text-lg font-bold font-['Space_Grotesk'] text-black leading-tight">
              Quản lý ca làm
            </h3>
          </div>

          <div className="space-y-1">
            <MenuItem
              id="sap-xep-lich"
              title="Sắp xếp lịch làm"
              icon={CalendarDaysIcon}
            />

            <MenuItem
              id="duyet-yeu-cau"
              title="Duyệt yêu cầu đổi ca"
              icon={ArrowPathIcon}
            />
          </div>
        </div>
      )}

      {/* 🔥 Quản lý tiền lương */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="px-4 py-2">
            <h3 className="text-lg font-bold font-['Space_Grotesk'] text-black leading-tight">
              Quản lý tiền lương
            </h3>
          </div>

          <div className="space-y-1">
            <MenuItem
              id="thong-ke-luong"
              title="Thống kê tiền lương"
              icon={ChartBarIcon}
            />

            <MenuItem
              id="dieu-chinh-luong"
              title="Điều chỉnh lương"
              icon={CurrencyDollarIcon}
            />
          </div>
        </div>
      )}

      {/* 🔥 Quản lý nhân viên */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="px-4 py-2">
            <h3 className="text-lg font-bold font-['Space_Grotesk'] text-black leading-tight">
              Quản lý nhân viên
            </h3>
          </div>

          <div className="space-y-1">
            <MenuItem
              id="thong-tin-nhan-vien"
              title="Thông tin nhân viên"
              icon={IdentificationIcon}
            />
          </div>
        </div>
      )}

      {/* 🔥 Collapsed State - Larger icons */}
      {isCollapsed && (
        <div className="space-y-3">
          <MenuItem
            id="sap-xep-lich"
            title="Sắp xếp lịch làm"
            icon={CalendarDaysIcon}
          />
          <MenuItem
            id="duyet-yeu-cau"
            title="Duyệt yêu cầu đổi ca"
            icon={ArrowPathIcon}
          />
          <MenuItem
            id="thong-ke-luong"
            title="Thống kê tiền lương"
            icon={ChartBarIcon}
          />
          <MenuItem
            id="dieu-chinh-luong"
            title="Điều chỉnh lương"
            icon={CurrencyDollarIcon}
          />
          <MenuItem
            id="thong-tin-nhan-vien"
            title="Thông tin nhân viên"
            icon={IdentificationIcon}
          />
        </div>
      )}
    </div>
  );
};
