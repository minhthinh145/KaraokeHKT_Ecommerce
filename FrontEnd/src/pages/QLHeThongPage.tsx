import React, { useState } from "react";
import { AdminLayout } from "../components/admins/uiForAll/AdminLayout";
import { QLHeThongSidebar } from "../components/admins/qlHeThong/QLHeThongSideBar";
import QLHeThongManagement from "../components/admins/qlHeThong/TaiKhoanManagement/NhanVienAccountManagement";
import { KhachHangManagement } from "../components/admins/qlHeThong/TaiKhoanManagement/KhachHangManagement";
import { AdminAccountManagement } from "../components/admins/qlHeThong/TaiKhoanManagement/AdminAccountManagement";
type ActiveTab = "nhan-vien" | "khach-hang" | "quan-ly";

export const QLHeThongPage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<ActiveTab>("nhan-vien");
  const [isSidebarCollapsed, setIsSidebarCollapsed] = useState(false);

  const renderTabContent = () => {
    switch (activeTab) {
      case "nhan-vien":
        return <QLHeThongManagement />;

      case "khach-hang":
        return <KhachHangManagement />;

      case "quan-ly":
        return <AdminAccountManagement />;

      default:
        return <div>Tab không tồn tại</div>;
    }
  };

  return (
    <AdminLayout
      sidebarContent={
        <QLHeThongSidebar
          isCollapsed={isSidebarCollapsed}
          activeTab={activeTab}
          onTabChange={setActiveTab}
        />
      }
      pageTitle="Quản lý hệ thống"
      searchPlaceholder="Tìm kiếm..."
      showSearch={false} // Disable global search since we have component-level search
      showFilter={false} // Disable global filter since we have component-level filters
    >
      {renderTabContent()}
    </AdminLayout>
  );
};
