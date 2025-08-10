import React, { useState } from "react";
import { AddAccountModal } from "./featureComponents/AddAccountModal";
import { StatsCardHelpers, StatsCards } from "../../uiForAll/StatsCards";
import { NhanVienAccountTable } from "./Table/NhanVienAccountTable";
import type { useQLHeThong } from "../../../../hooks/useQLHeThong";
import { AccountFilterBar } from "../../uiForAll/AccountFilterBar";
import { RoleDescriptions, EMPLOYEE_ROLES } from "../../../../constants/auth";

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

  const roleOptions = EMPLOYEE_ROLES.map((r) => ({
    value: r,
    label: RoleDescriptions[r] || r,
  }));

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

      {/* Filter Bar (chuẩn hoá) */}
      <AccountFilterBar
        colorTheme="blue"
        searchPlaceholder="🔍 Tìm kiếm theo tên, email, username..."
        searchValue={ui.searchQuery}
        onSearchChange={actions.setSearchQuery}
        showRoleFilter
        roleValue={ui.filters.loaiTaiKhoan || ""}
        roleOptions={roleOptions}
        roleAllLabel="Tất cả nhân viên"
        onRoleChange={actions.setRoleFilter}
        showStatusFilter
        statusValue={ui.filters.trangThai || ""}
        onStatusChange={actions.setStatusFilter}
        onRefresh={actions.loadNhanVien}
        refreshing={loading.nhanVien}
        onClearAll={actions.clearAllFilters}
      />

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
