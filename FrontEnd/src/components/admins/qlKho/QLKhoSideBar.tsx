import React from "react";
import {
  ChartBarIcon,
  CubeIcon,
  DocumentMinusIcon,
  ClipboardIcon,
} from "@heroicons/react/24/outline";

type TabParent = "chuc-nang-thong-ke" | "chuc-nang-quan-ly";
type TabChild =
  | "thong-ke-kho-hang"
  | "thong-ke-nhap-xuat"
  | "quan-ly-hang-hoa"
  | "don-huy-hang"
  | "duyet-don-nhap-hang";

interface QLKhoSidebarProps {
  isCollapsed: boolean;
  activeParent: TabParent;
  activeTab: TabChild;
  onParentChange: (parent: TabParent) => void;
  onTabChange: (tab: TabChild) => void;
}

const THONG_KE_MENUS: { id: TabChild; title: string; icon: any }[] = [
  {
    id: "thong-ke-kho-hang",
    title: "Thống kê kho hàng",
    icon: ChartBarIcon,
  },
  {
    id: "thong-ke-nhap-xuat",
    title: "Thống kê nhập xuất",
    icon: ChartBarIcon,
  },
];

const QUAN_LY_MENUS: { id: TabChild; title: string; icon: any }[] = [
  {
    id: "quan-ly-hang-hoa",
    title: "Quản lý hàng hóa",
    icon: CubeIcon,
  },
  {
    id: "don-huy-hang",
    title: "Đơn hủy hàng",
    icon: DocumentMinusIcon,
  },
  {
    id: "duyet-don-nhap-hang",
    title: "Duyệt đơn nhập hàng",
    icon: ClipboardIcon,
  },
];

export const QLKhoSidebar: React.FC<QLKhoSidebarProps> = ({
  isCollapsed,
  activeParent,
  activeTab,
  onParentChange,
  onTabChange,
}) => {
  // Tiêu đề menu con theo tab cha
  const getChildTitle = () => {
    switch (activeParent) {
      case "chuc-nang-thong-ke":
        return "Chức năng thống kê";
      case "chuc-nang-quan-ly":
        return "Chức năng quản lý";
      default:
        return "";
    }
  };

  const renderMenu = (menus: typeof THONG_KE_MENUS | typeof QUAN_LY_MENUS) =>
    menus.map(({ id, title, icon: Icon }) => (
      <button
        key={id}
        onClick={() => onTabChange(id)}
        className={`w-full flex items-center gap-3 px-4 py-2 rounded-lg text-base font-medium transition
          ${
            activeTab === id
              ? activeParent === "chuc-nang-thong-ke"
                ? "bg-blue-50 text-blue-700 font-semibold shadow"
                : "bg-green-50 text-green-700 font-semibold shadow"
              : "text-gray-700 hover:bg-blue-50 hover:text-blue-700"
          }
        `}
      >
        <Icon className="w-5 h-5" />
        <span>{title}</span>
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
              activeParent === "chuc-nang-thong-ke"
                ? "bg-blue-100 text-blue-700 ring-2 ring-blue-300"
                : "bg-white text-gray-700 hover:bg-gray-50"
            }`}
            onClick={() => onParentChange("chuc-nang-thong-ke")}
          >
            Chức năng thống kê
          </button>
          <button
            className={`w-full py-2 rounded-lg font-bold text-base transition ${
              activeParent === "chuc-nang-quan-ly"
                ? "bg-green-100 text-green-700 ring-2 ring-green-300"
                : "bg-white text-gray-700 hover:bg-gray-50"
            }`}
            onClick={() => onParentChange("chuc-nang-quan-ly")}
          >
            Chức năng quản lý
          </button>
        </div>
      </div>

      {/* Menu con - có tiêu đề riêng cho từng tab cha */}
      {!isCollapsed && (
        <div className="space-y-2">
          <div className="mb-1 pl-2 text-xs font-semibold text-gray-500 uppercase tracking-wider">
            {getChildTitle()}
          </div>
          <div className="bg-white border border-neutral-200 rounded-xl shadow-sm p-2">
            {activeParent === "chuc-nang-thong-ke" &&
              renderMenu(THONG_KE_MENUS)}
            {activeParent === "chuc-nang-quan-ly" && renderMenu(QUAN_LY_MENUS)}
          </div>
        </div>
      )}
    </div>
  );
};
