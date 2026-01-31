import React from "react";
import {
  ChartBarIcon,
  CubeIcon,
  DocumentMinusIcon,
  ClipboardIcon,
  ArchiveBoxIcon,
} from "@heroicons/react/24/outline";
import {
  SideBarForAdmin,
  type SideBarForAdminProps,
} from "../uiForAll/SideBarForAdmin";

type TabParent = "chuc-nang-thong-ke" | "chuc-nang-quan-ly";
type TabChild =
  | "thong-ke-kho-hang"
  | "thong-ke-nhap-xuat"
  | "quan-ly-hang-hoa"
  | "don-huy-hang"
  | "duyet-don-nhap-hang";

export interface QLKhoSidebarProps {
  isCollapsed: boolean;
  activeParent: TabParent;
  activeTab: TabChild;
  onParentChange: (parent: TabParent) => void;
  onTabChange: (tab: TabChild) => void;
}

const QLKHO_TREE: SideBarForAdminProps<TabParent, TabChild>["treeData"] = [
  {
    id: "chuc-nang-thong-ke",
    title: "Chức năng thống kê",
    icon: ChartBarIcon,
    color: "blue",
    children: [
      {
        id: "thong-ke-kho-hang",
        title: "Thống kê kho hàng",
        icon: ArchiveBoxIcon,
      },
      {
        id: "thong-ke-nhap-xuat",
        title: "Thống kê nhập xuất",
        icon: ChartBarIcon,
      },
    ],
  },
  {
    id: "chuc-nang-quan-ly",
    title: "Chức năng quản lý",
    icon: CubeIcon,
    color: "green",
    children: [
      { id: "quan-ly-hang-hoa", title: "Quản lý hàng hóa", icon: CubeIcon },
      { id: "don-huy-hang", title: "Đơn hủy hàng", icon: DocumentMinusIcon },
      {
        id: "duyet-don-nhap-hang",
        title: "Duyệt đơn nhập hàng",
        icon: ClipboardIcon,
      },
    ],
  },
];

export const QLKhoSidebar: React.FC<QLKhoSidebarProps> = (props) => {
  return (
    <SideBarForAdmin<TabParent, TabChild>
      {...props}
      treeData={QLKHO_TREE}
      sidebarTitle="Danh mục"
    />
  );
};
