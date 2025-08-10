import React from "react";
import {
  CalendarDaysIcon,
  ArrowPathIcon,
  ChartBarIcon,
  CurrencyDollarIcon,
  IdentificationIcon,
} from "@heroicons/react/24/outline";

type TabParent = "quan-ly-ca-lam" | "quan-ly-tien-luong" | "quan-ly-nhan-vien";
type TabChild =
  | "sap-xep-lich"
  | "duyet-yeu-cau"
  | "thong-ke-luong"
  | "dieu-chinh-luong"
  | "thong-tin-nhan-vien";

interface QLNhanSuSidebarProps {
  isCollapsed: boolean;
  activeParent: TabParent;
  activeTab: TabChild;
  onParentChange: (parent: TabParent) => void;
  onTabChange: (tab: TabChild) => void;
}

const CA_LAM_MENUS: { id: TabChild; title: string; icon: any }[] = [
  {
    id: "sap-xep-lich",
    title: "Sắp xếp lịch làm",
    icon: CalendarDaysIcon,
  },
  {
    id: "duyet-yeu-cau",
    title: "Duyệt yêu cầu đổi ca",
    icon: ArrowPathIcon,
  },
];

const TIEN_LUONG_MENUS: { id: TabChild; title: string; icon: any }[] = [
  {
    id: "thong-ke-luong",
    title: "Thống kê tiền lương",
    icon: ChartBarIcon,
  },
  {
    id: "dieu-chinh-luong",
    title: "Điều chỉnh lương",
    icon: CurrencyDollarIcon,
  },
];

const NHAN_VIEN_MENUS: { id: TabChild; title: string; icon: any }[] = [
  {
    id: "thong-tin-nhan-vien",
    title: "Thông tin nhân viên",
    icon: IdentificationIcon,
  },
];

export const QLNhanSuSidebar: React.FC<QLNhanSuSidebarProps> = ({
  isCollapsed,
  activeParent,
  activeTab,
  onParentChange,
  onTabChange,
}) => {
  const renderMenu = (
    menus:
      | typeof CA_LAM_MENUS
      | typeof TIEN_LUONG_MENUS
      | typeof NHAN_VIEN_MENUS
  ) =>
    menus.map(({ id, title, icon: Icon }) => (
      <button
        key={id}
        onClick={() => onTabChange(id)}
        className={`w-full min-h-[44px] px-4 py-2 rounded-lg flex items-center gap-4 transition-colors ${
          activeTab === id
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
    ));

  return (
    <div className="p-4 space-y-6">
      {/* Tabs cha - làm nổi bật cụm này */}
      <div className="mb-4">
        <div className="mb-2 text-xs font-bold text-gray-500 uppercase tracking-widest pl-1">
          Danh mục chức năng
        </div>
        <div className="flex flex-col gap-2 bg-gradient-to-r from-blue-50 via-green-50 to-purple-50 border border-blue-200 rounded-xl shadow-md p-3">
          <button
            className={`w-full py-2 rounded-lg font-bold text-base transition ${
              activeParent === "quan-ly-ca-lam"
                ? "bg-blue-100 text-blue-700 ring-2 ring-blue-300"
                : "bg-white text-gray-700 hover:bg-gray-50"
            }`}
            onClick={() => onParentChange("quan-ly-ca-lam")}
          >
            Quản lý ca làm
          </button>
          <button
            className={`w-full py-2 rounded-lg font-bold text-base transition ${
              activeParent === "quan-ly-tien-luong"
                ? "bg-green-100 text-green-700 ring-2 ring-green-300"
                : "bg-white text-gray-700 hover:bg-gray-50"
            }`}
            onClick={() => onParentChange("quan-ly-tien-luong")}
          >
            Quản lý tiền lương
          </button>
          <button
            className={`w-full py-2 rounded-lg font-bold text-base transition ${
              activeParent === "quan-ly-nhan-vien"
                ? "bg-purple-100 text-purple-700 ring-2 ring-purple-300"
                : "bg-white text-gray-700 hover:bg-gray-50"
            }`}
            onClick={() => onParentChange("quan-ly-nhan-vien")}
          >
            Quản lý nhân viên
          </button>
        </div>
      </div>

      {/* Menu con - làm nổi bật hơn, gọn gàng hơn */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="mb-1 pl-2 text-xs font-semibold text-gray-500 uppercase tracking-wider">
            Chức năng cụ thể
          </div>
          <div className="bg-white border border-neutral-200 rounded-xl shadow-sm p-2">
            {activeParent === "quan-ly-ca-lam" &&
              CA_LAM_MENUS.map(({ id, title, icon: Icon }) => (
                <button
                  key={id}
                  onClick={() => onTabChange(id)}
                  className={`w-full flex items-center gap-3 px-4 py-2 rounded-lg text-base font-medium transition
                    ${
                      activeTab === id
                        ? "bg-blue-50 text-blue-700 font-semibold shadow"
                        : "text-gray-700 hover:bg-blue-50 hover:text-blue-700"
                    }
                  `}
                >
                  <Icon className="w-5 h-5" />
                  <span>{title}</span>
                </button>
              ))}

            {activeParent === "quan-ly-tien-luong" &&
              TIEN_LUONG_MENUS.map(({ id, title, icon: Icon }) => (
                <button
                  key={id}
                  onClick={() => onTabChange(id)}
                  className={`w-full flex items-center gap-3 px-4 py-2 rounded-lg text-base font-medium transition
                    ${
                      activeTab === id
                        ? "bg-green-50 text-green-700 font-semibold shadow"
                        : "text-gray-700 hover:bg-green-50 hover:text-green-700"
                    }
                  `}
                >
                  <Icon className="w-5 h-5" />
                  <span>{title}</span>
                </button>
              ))}

            {activeParent === "quan-ly-nhan-vien" &&
              NHAN_VIEN_MENUS.map(({ id, title, icon: Icon }) => (
                <button
                  key={id}
                  onClick={() => onTabChange(id)}
                  className={`w-full flex items-center gap-3 px-4 py-2 rounded-lg text-base font-medium transition
                    ${
                      activeTab === id
                        ? "bg-purple-50 text-purple-700 font-semibold shadow"
                        : "text-gray-700 hover:bg-purple-50 hover:text-purple-700"
                    }
                  `}
                >
                  <Icon className="w-5 h-5" />
                  <span>{title}</span>
                </button>
              ))}
          </div>
        </div>
      )}
    </div>
  );
};
