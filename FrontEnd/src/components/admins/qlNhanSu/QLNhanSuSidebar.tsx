import {
  CalendarDaysIcon,
  ArrowPathIcon,
  ChartBarIcon,
  CurrencyDollarIcon,
  IdentificationIcon,
} from "@heroicons/react/24/outline";
import {
  SideBarForAdmin,
  type SideBarForAdminProps,
} from "../uiForAll/SideBarForAdmin";

type TabParent = "quan-ly-ca-lam" | "quan-ly-tien-luong" | "quan-ly-nhan-vien";
type TabChild =
  | "sap-xep-lich"
  | "duyet-yeu-cau"
  | "thong-ke-luong"
  | "dieu-chinh-luong"
  | "thong-tin-nhan-vien";

export interface QLNhanSuSidebarProps {
  isCollapsed: boolean;
  activeParent: TabParent;
  activeTab: TabChild;
  onParentChange: (parent: TabParent) => void;
  onTabChange: (tab: TabChild) => void;
}

const QLNS_TREE: SideBarForAdminProps<TabParent, TabChild>["treeData"] = [
  {
    id: "quan-ly-ca-lam",
    title: "Quản lý ca làm",
    icon: CalendarDaysIcon,
    color: "blue",
    children: [
      { id: "sap-xep-lich", title: "Sắp xếp lịch làm", icon: CalendarDaysIcon },
      {
        id: "duyet-yeu-cau",
        title: "Duyệt yêu cầu đổi ca",
        icon: ArrowPathIcon,
      },
    ],
  },
  {
    id: "quan-ly-tien-luong",
    title: "Quản lý tiền lương",
    icon: CurrencyDollarIcon,
    color: "green",
    children: [
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
    ],
  },
  {
    id: "quan-ly-nhan-vien",
    title: "Quản lý nhân viên",
    icon: IdentificationIcon,
    color: "purple",
    children: [
      {
        id: "thong-tin-nhan-vien",
        title: "Thông tin nhân viên",
        icon: IdentificationIcon,
      },
    ],
  },
];

export const QLNhanSuSidebar: React.FC<QLNhanSuSidebarProps> = (props) => {
  return (
    <SideBarForAdmin {...props} treeData={QLNS_TREE} sidebarTitle="Danh mục" />
  );
};
