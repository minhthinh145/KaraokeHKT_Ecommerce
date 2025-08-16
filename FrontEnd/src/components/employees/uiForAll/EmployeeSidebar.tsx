import React from "react";
import {
  ClipboardDocumentListIcon,
  ArrowsRightLeftIcon,
  ChartBarIcon,
} from "@heroicons/react/24/outline";
import {
  SideBarForAdmin,
  type SideBarForAdminProps,
} from "../../../components/admins/uiForAll/SideBarForAdmin";

// Các parent (nhóm) của sidebar nhân viên
export type EmpParent = "quan-ly-lich" | "bao-cao";
// Các tab con
export type EmpTab =
  | "lich-lam-viec"
  | "yeu-cau-chuyen-ca"
  | "bao-cao-tong-quan";

export interface EmployeeSidebarProps {
  isCollapsed: boolean;
  activeParent: EmpParent;
  activeTab: EmpTab;
  onParentChange: (p: EmpParent) => void;
  onTabChange: (t: EmpTab) => void;
}

const EMPLOYEE_TREE: SideBarForAdminProps<EmpParent, EmpTab>["treeData"] = [
  {
    id: "quan-ly-lich",
    title: "Lịch & Ca",
    icon: ClipboardDocumentListIcon,
    color: "blue",
    children: [
      {
        id: "lich-lam-viec",
        title: "Lịch làm việc",
        icon: ClipboardDocumentListIcon,
      },
      {
        id: "yeu-cau-chuyen-ca",
        title: "Yêu cầu chuyển ca",
        icon: ArrowsRightLeftIcon,
      },
    ],
  },
  {
    id: "bao-cao",
    title: "Báo cáo",
    icon: ChartBarIcon,
    color: "orange",
    children: [
      {
        id: "bao-cao-tong-quan",
        title: "Tổng quan",
        icon: ChartBarIcon,
      },
    ],
  },
];

export const EmployeeSidebar: React.FC<EmployeeSidebarProps> = (props) => {
  return (
    <SideBarForAdmin
      isCollapsed={props.isCollapsed}
      activeParent={props.activeParent}
      activeTab={props.activeTab}
      onParentChange={props.onParentChange}
      onTabChange={props.onTabChange}
      treeData={EMPLOYEE_TREE}
      sidebarTitle="Nhân viên"
    />
  );
};
