import React, { useState } from "react";
import { AdminLayout } from "../../components/admins/uiForAll/AdminLayout";
import { QLPhongSidebar } from "../../components/admins/qlPhong/QLPhongSidebar";
import { LoaiPhongManagement } from "../../components/admins/qlPhong/LoaiPhongManagement/LoaiPhongManagement";
import { PhongHatManagement } from "../../components/admins/qlPhong/PhongHatManagement/PhongHatManagement";

// Định nghĩa các tab cha và tab con
type TabParent = "chuc-nang-thong-ke" | "chuc-nang-quan-ly";
type TabChild = "thong-ke-lich-su" | "quan-ly-phong-hat" | "quan-ly-loai-phong";

export const QLPhongPage: React.FC = () => {
  const [activeParent, setActiveParent] =
    useState<TabParent>("chuc-nang-thong-ke");
  const [activeTab, setActiveTab] = useState<TabChild>("thong-ke-lich-su");

  // Hàm render nội dung theo tab con
  const renderTabContent = () => {
    switch (activeTab) {
      case "thong-ke-lich-su":
        return (
          <div className="bg-white rounded-lg border border-neutral-200 p-6">
            <h1 className="text-2xl font-bold mb-2 text-black font-['Space_Grotesk']">
              Thống kê lịch sử hoạt động
            </h1>
            <p className="text-gray-600">
              📊 Trang thống kê lịch sử phòng đang được phát triển...
            </p>
          </div>
        );
      case "quan-ly-phong-hat":
        return <PhongHatManagement />;
      case "quan-ly-loai-phong":
        return <LoaiPhongManagement />;
      default:
        return (
          <div className="bg-white rounded-lg border border-neutral-200 p-6">
            <p className="text-gray-600">
              Vui lòng chọn một tính năng từ menu bên trái.
            </p>
          </div>
        );
    }
  };

  return (
    <AdminLayout
      sidebarContent={
        <QLPhongSidebar
          isCollapsed={false}
          activeParent={activeParent}
          activeTab={activeTab}
          onParentChange={setActiveParent}
          onTabChange={setActiveTab}
        />
      }
      showFilter={false}
      showSearch={false}
      pageTitle="Quản lý phòng hát"
      searchPlaceholder="Tìm kiếm..."
    >
      {renderTabContent()}
    </AdminLayout>
  );
};
