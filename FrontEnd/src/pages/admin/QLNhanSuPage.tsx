import React, { useState } from "react";
import { AdminLayout } from "../../components/admins/uiForAll/AdminLayout";
import { QLNhanSuSidebar } from "../../components/admins/qlNhanSu/QLNhanSuSidebar";
import { NhanVienManagement } from "../../components/admins/qlNhanSu/NhanVienManagement/NhanVienManagement";
import { TienLuongManagement } from "../../components/admins/qlNhanSu/TienLuongManagement/TienLuongManagement";
import { LichLamViecManagement } from "../../components/admins/qlNhanSu/LichLamViecManagement/LichLamViecManagement";
import { YeuCauChuyenCaManagement } from "../../components/admins/qlNhanSu/PheDuyetYeuCau";

type TabParent = "quan-ly-ca-lam" | "quan-ly-tien-luong" | "quan-ly-nhan-vien";
type TabChild =
  | "sap-xep-lich"
  | "duyet-yeu-cau"
  | "thong-ke-luong"
  | "dieu-chinh-luong"
  | "thong-tin-nhan-vien";
export const QLNhanSuPage: React.FC = () => {
  const [activeParent, setActiveParent] = useState<TabParent>("quan-ly-ca-lam");
  const [activeTab, setActiveTab] = useState<TabChild>("sap-xep-lich");

  const renderTabContent = () => {
    switch (activeTab) {
      case "thong-tin-nhan-vien":
        return <NhanVienManagement />;

      case "sap-xep-lich":
        return <LichLamViecManagement />;

      case "duyet-yeu-cau":
        return <YeuCauChuyenCaManagement />;

      case "thong-ke-luong":
        return (
          <div className="space-y-6">
            <div className="flex items-center justify-between">
              <h1 className="text-2xl font-bold text-black font-['Space_Grotesk']">
                Thống kê tiền lương
              </h1>
              <button className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors">
                Xuất báo cáo
              </button>
            </div>

            <div className="bg-white rounded-lg border border-neutral-200 p-6">
              <p className="text-gray-600">
                📊 Trang thống kê tiền lương đang được phát triển...
              </p>
            </div>
          </div>
        );

      case "dieu-chinh-luong":
        return <TienLuongManagement />;

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
        <QLNhanSuSidebar
          isCollapsed={false}
          activeParent={activeParent}
          activeTab={activeTab}
          onParentChange={setActiveParent}
          onTabChange={setActiveTab}
        />
      }
      showFilter={false}
      showSearch={false}
      pageTitle="Quản lý nhân sự"
      searchPlaceholder="Tìm kiếm..."
    >
      {renderTabContent()}
    </AdminLayout>
  );
};
