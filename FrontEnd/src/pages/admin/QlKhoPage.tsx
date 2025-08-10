import React, { useState } from "react";
import { AdminLayout } from "../../components/admins/uiForAll/AdminLayout";
import { QLKhoSidebar } from "../../components/admins/qlKho/QLKhoSideBar";

// Tab cha
type TabParent = "chuc-nang-thong-ke" | "chuc-nang-quan-ly";
// Tab con
type TabChild =
  | "thong-ke-kho-hang"
  | "thong-ke-nhap-xuat"
  | "quan-ly-hang-hoa"
  | "don-huy-hang"
  | "duyet-don-nhap-hang";

export const QLKhoPage: React.FC = () => {
  const [activeParent, setActiveParent] =
    useState<TabParent>("chuc-nang-thong-ke");
  const [activeTab, setActiveTab] = useState<TabChild>("thong-ke-kho-hang");

  // Render nội dung từng tab con
  const renderTabContent = () => {
    switch (activeTab) {
      case "thong-ke-kho-hang":
        return (
          <div className="space-y-6">
            <h1 className="text-2xl font-bold text-black font-['Space_Grotesk']">
              Thống kê kho hàng
            </h1>
            <div className="bg-white rounded-lg border border-neutral-200 p-6">
              <p className="text-gray-600">
                📦 Trang thống kê kho hàng đang được phát triển...
              </p>
            </div>
          </div>
        );
      case "thong-ke-nhap-xuat":
        return (
          <div className="space-y-6">
            <h1 className="text-2xl font-bold text-black font-['Space_Grotesk']">
              Thống kê nhập xuất
            </h1>
            <div className="bg-white rounded-lg border border-neutral-200 p-6">
              <p className="text-gray-600">
                📊 Trang thống kê nhập xuất đang được phát triển...
              </p>
            </div>
          </div>
        );
      case "quan-ly-hang-hoa":
        return (
          <div className="space-y-6">
            <h1 className="text-2xl font-bold text-black font-['Space_Grotesk']">
              Quản lý hàng hóa
            </h1>
            <div className="bg-white rounded-lg border border-neutral-200 p-6">
              <p className="text-gray-600">
                🗃️ Trang quản lý hàng hóa đang được phát triển...
              </p>
            </div>
          </div>
        );
      case "don-huy-hang":
        return (
          <div className="space-y-6">
            <h1 className="text-2xl font-bold text-black font-['Space_Grotesk']">
              Đơn hủy hàng
            </h1>
            <div className="bg-white rounded-lg border border-neutral-200 p-6">
              <p className="text-gray-600">
                ❌ Trang đơn hủy hàng đang được phát triển...
              </p>
            </div>
          </div>
        );
      case "duyet-don-nhap-hang":
        return (
          <div className="space-y-6">
            <h1 className="text-2xl font-bold text-black font-['Space_Grotesk']">
              Duyệt đơn nhập hàng
            </h1>
            <div className="bg-white rounded-lg border border-neutral-200 p-6">
              <p className="text-gray-600">
                ✅ Trang duyệt đơn nhập hàng đang được phát triển...
              </p>
            </div>
          </div>
        );
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
        <QLKhoSidebar
          isCollapsed={false}
          activeParent={activeParent}
          activeTab={activeTab}
          onParentChange={setActiveParent}
          onTabChange={setActiveTab}
        />
      }
      pageTitle="Quản lý kho"
      searchPlaceholder="Tìm kiếm..."
      showFilter={false}
      showSearch={false}
    >
      {renderTabContent()}
    </AdminLayout>
  );
};
