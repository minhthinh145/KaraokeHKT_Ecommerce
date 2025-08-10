import React, { useState } from "react";
import { AdminLayout } from "../../components/admins/uiForAll/AdminLayout";
import { QLHeThongSidebar } from "../../components/admins/qlHeThong/QLHeThongSideBar";
import { NhanVienAccountManagement } from "../../components/admins/qlHeThong/TaiKhoanManagement/NhanVienAccountManagement";
import { KhachHangAccountManagement } from "../../components/admins/qlHeThong/TaiKhoanManagement/KhachHangAccountManagement";
import { AdminAccountManagement } from "../../components/admins/qlHeThong/TaiKhoanManagement/AdminAccountManagement";
import { useQLHeThong } from "../../hooks/useQLHeThong";
type ActiveTab = "nhan-vien" | "khach-hang" | "quan-ly";

export const QLHeThongPage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<ActiveTab>("nhan-vien");
  const [isSidebarCollapsed, setIsSidebarCollapsed] = useState(false);
  const qlHeThong = useQLHeThong();

  const renderTabContent = () => {
    switch (activeTab) {
      case "nhan-vien":
        return <NhanVienAccountManagement qlHeThong={qlHeThong} />;

      case "khach-hang":
        return <KhachHangAccountManagement qlHeThong={qlHeThong} />;

      case "quan-ly":
        return <AdminAccountManagement qlHeThong={qlHeThong} />;

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
