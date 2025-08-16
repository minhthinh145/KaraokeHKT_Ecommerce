import React from "react";
import { useQLHeThong } from "../../../../hooks/useQLHeThong";
import { KhachHangAccountTable } from "./Table/KhachHangAccountTable";
import { StatsCardHelpers, StatsCards } from "../../uiForAll/StatsCards";
import { AccountFilterBar } from "../../uiForAll/AccountFilterBar";

export const KhachHangAccountManagement: React.FC<{
  qlHeThong: ReturnType<typeof useQLHeThong>;
}> = ({ qlHeThong }) => {
  const { khachHangData, loading, ui, actions, lockHandlers } = qlHeThong;

  // Tính toán số liệu thống kê
  const totalKhachHang = khachHangData.length;
  const filteredKhachHang = ui.filteredKhachHang.length;
  const activeKhachHang = ui.filteredKhachHang.filter(
    (item) => !item.daBiKhoa && item.daKichHoat
  ).length;
  const lockedKhachHang = ui.filteredKhachHang.filter(
    (item) => item.daBiKhoa
  ).length;
  const statsCard = [
    StatsCardHelpers.totalCard(totalKhachHang, "khách hàng"),
    StatsCardHelpers.filteredCard(filteredKhachHang),
    StatsCardHelpers.activeCard(activeKhachHang),
    StatsCardHelpers.lockedCard(lockedKhachHang),
  ];
  return (
    <div className="space-y-6">
      {/* Header - GIỮ NGUYÊN */}
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-xl font-semibold text-gray-900">
            👤 Quản lý tài khoản khách hàng
          </h2>
          <p className="mt-1 text-sm text-gray-600">
            Quản lý thông tin tài khoản và khách hàng trong hệ thống
          </p>
        </div>
      </div>

      {/* Stats Cards */}
      <StatsCards cards={statsCard} gridCols={4} />

      {/* Filter Bar - GIỮ NGUYÊN */}
      <AccountFilterBar
        colorTheme="green"
        searchPlaceholder="Tìm kiếm theo tên, email, username..."
        searchValue={ui.searchQuery}
        onSearchChange={actions.setSearchQuery}
        showRoleFilter={false}
        showStatusFilter
        statusValue={ui.filters.trangThai || ""}
        onStatusChange={actions.setStatusFilter}
        onRefresh={() => actions.loadKhachHang && actions.loadKhachHang()}
        refreshing={loading.khachHang}
        onClearAll={actions.clearAllFilters}
      />
      {/* Table */}
      <KhachHangAccountTable
        data={ui.filteredKhachHang}
        loading={loading.khachHang}
        onLockToggle={lockHandlers.khachHang.lockToggle}
      />
    </div>
  );
};
