import React from "react";
import { useQLHeThong } from "../../../../hooks/useQLHeThong";
import { KhachHangAccountTable } from "./Table/KhachHangAccountTable";
import { StatsCardHelpers, StatsCards } from "../../uiForAll/StatsCards";

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
        <button className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors">
          Thêm khách hàng
        </button>
      </div>

      {/* Stats Cards */}
      <StatsCards cards={statsCard} gridCols={4} />

      {/* Filter Bar - GIỮ NGUYÊN */}
      <div className="bg-white p-4 rounded-lg border border-neutral-200">
        <div className="flex items-center gap-4">
          {/* Search */}
          <div className="flex-1">
            <input
              type="text"
              placeholder="Tìm kiếm theo tên, email, username..."
              value={ui.searchQuery}
              onChange={(e) => actions.setSearchQuery(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500"
            />
          </div>

          {/* Filter by Status */}
          <select
            value={ui.filters.trangThai || ""}
            onChange={(e) => actions.setStatusFilter(e.target.value)}
            className="px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500"
          >
            <option value="">Tất cả trạng thái</option>
            <option value="active">Hoạt động</option>
            <option value="inactive">Chưa kích hoạt</option>
            <option value="locked">Bị khóa</option>
          </select>
        </div>
      </div>

      {/* Table */}
      <KhachHangAccountTable
        data={ui.filteredKhachHang}
        loading={loading.khachHang}
        onLockToggle={lockHandlers.khachHang.lockToggle}
      />
    </div>
  );
};
