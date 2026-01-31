import {
  ChartBarIcon,
  BuildingOffice2Icon,
  ArrowPathIcon,
} from "@heroicons/react/24/outline";
import {
  SideBarForAdmin,
  type SideBarForAdminProps,
} from "../uiForAll/SideBarForAdmin";

type TabParent = "chuc-nang-thong-ke" | "chuc-nang-quan-ly";
type TabChild = "thong-ke-lich-su" | "quan-ly-phong-hat" | "quan-ly-loai-phong";

export interface QLPhongSidebarProps {
  isCollapsed: boolean;
  activeParent: TabParent;
  activeTab: TabChild;
  onParentChange: (parent: TabParent) => void;
  onTabChange: (tab: TabChild) => void;
}

const QLPHONG_TREE: SideBarForAdminProps<TabParent, TabChild>["treeData"] = [
  {
    id: "chuc-nang-thong-ke",
    title: "Chức năng thống kê",
    icon: ChartBarIcon,
    color: "orange",
    children: [
      {
        id: "thong-ke-lich-su",
        title: "Thống kê lịch sử hoạt động",
        icon: BuildingOffice2Icon,
      },
    ],
  },
  {
    id: "chuc-nang-quan-ly",
    title: "Chức năng quản lý",
    icon: BuildingOffice2Icon,
    color: "blue",
    children: [
      {
        id: "quan-ly-phong-hat",
        title: "Quản lý phòng hát",
        icon: BuildingOffice2Icon,
      },
      {
        id: "quan-ly-loai-phong",
        title: "Quản lý loại phòng",
        icon: ArrowPathIcon,
      },
    ],
  },
];

export const QLPhongSidebar: React.FC<QLPhongSidebarProps> = (props) => {
  return (
    <SideBarForAdmin
      {...props}
      treeData={QLPHONG_TREE}
      sidebarTitle="Quản lý phòng hát"
    />
  );
};
