import React, { useState } from "react";
import { AddAccountModal } from "./featureComponents/AddAccountModal";
import { StatsCardHelpers, StatsCards } from "../../uiForAll/StatsCards";
import { NhanVienAccountTable } from "./Table/NhanVienAccountTable";
import type { useQLHeThong } from "../../../../hooks/useQLHeThong";

export const NhanVienAccountManagement: React.FC<{
  qlHeThong: ReturnType<typeof useQLHeThong>;
}> = ({ qlHeThong }) => {
  const { ui, loading, errors, actions, handlers, nhanVienData, lockHandlers } =
    qlHeThong;
  const [showModal, setShowModal] = useState(false);

  // Stats cho nhân viên
  const totalNhanVien = nhanVienData.length;
  const filteredNhanVien = ui.filteredNhanVien.length;
  const activeNhanVien = ui.filteredNhanVien.filter(
    (item) => !item.daBiKhoa && item.daKichHoat
  ).length;
  const lockedNhanVien = ui.filteredNhanVien.filter(
    (item) => item.daBiKhoa
  ).length;

  const statsCards = [
    StatsCardHelpers.totalCard(totalNhanVien, "nhân viên"),
    StatsCardHelpers.filteredCard(filteredNhanVien),
    StatsCardHelpers.activeCard(activeNhanVien),
    StatsCardHelpers.lockedCard(lockedNhanVien),
  ];
  return (
    <div className="space-y-6">
      {/* Header - GIỮ NGUYÊN LAYOUT DÀI */}
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-xl font-semibold text-gray-900">
            🔐 Quản lý tài khoản nhân viên
          </h2>
          <p className="mt-1 text-sm text-gray-600">
            Quản lý thông tin tài khoản và nhân viên trong hệ thống
          </p>
        </div>
        <button
          onClick={() => setShowModal(true)}
          className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
        >
          Thay đổi tài khoản
        </button>
      </div>

      {/* Stats Cards - GIỮ NGUYÊN */}
      <StatsCards cards={statsCards} gridCols={4} />

      {/* Error Alert - GIỮ NGUYÊN */}
      {errors.nhanVien && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4">
          <div className="flex items-center">
            <div className="text-red-500 text-xl mr-3">⚠️</div>
            <div>
              <h3 className="text-red-800 font-semibold mb-1">Có lỗi xảy ra</h3>
              <p className="text-red-700">{errors.nhanVien}</p>
            </div>
            <button
              onClick={actions.clearErrors}
              className="ml-auto text-red-400 hover:text-red-600"
            >
              ✕
            </button>
          </div>
        </div>
      )}

      {/* Filter Bar - GIỮ NGUYÊN */}
      <div className="bg-white p-4 rounded-lg border border-neutral-200">
        <div className="flex items-center gap-4">
          {/* Search */}
          <div className="flex-1">
            <input
              type="text"
              placeholder="🔍 Tìm kiếm theo tên, email, username..."
              value={ui.searchQuery}
              onChange={(e) => actions.setSearchQuery(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            />
          </div>

          {/* Filter by Role */}
          <select
            value={ui.filters.loaiTaiKhoan || ""}
            onChange={(e) => actions.setRoleFilter(e.target.value)}
            className="px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="">Tất cả vai trò</option>
            {ui.filterOptions.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>

          {/* Filter by Status */}
          <select
            value={ui.filters.trangThai || ""}
            onChange={(e) => actions.setStatusFilter(e.target.value)}
            className="px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="">Tất cả trạng thái</option>
            <option value="active">Hoạt động</option>
            <option value="inactive">Chưa kích hoạt</option>
            <option value="locked">Bị khóa</option>
          </select>

          {/* Action Buttons */}
          <button
            onClick={actions.loadNhanVien}
            disabled={loading.nhanVien}
            className="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors disabled:opacity-50"
          >
            {loading.nhanVien ? "⏳" : "🔄"} Làm mới
          </button>

          <button
            onClick={actions.clearAllFilters}
            className="px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors"
          >
            🗑️ Xóa lọc
          </button>
        </div>

        <div className="mt-2 text-sm text-gray-600">
          Đang hiển thị:{" "}
          <span className="font-bold text-blue-600">{filteredNhanVien}</span> /{" "}
          {totalNhanVien} tài khoản
        </div>
      </div>

      <NhanVienAccountTable
        data={ui.filteredNhanVien}
        loading={loading.nhanVien}
        onLockToggle={lockHandlers.nhanVien.lockToggle}
      />

      <AddAccountModal
        visible={showModal}
        onCancel={() => setShowModal(false)}
        onSuccess={() => {
          setShowModal(false);
          actions.loadNhanVien(); // Refresh data
        }}
      />
    </div>
  );
};
