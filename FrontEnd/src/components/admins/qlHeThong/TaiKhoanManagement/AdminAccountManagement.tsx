import React, { useState } from "react";
import { useQLHeThong } from "../../../../hooks/useQLHeThong";
import { StatsCardHelpers, StatsCards } from "../../uiForAll/StatsCards";
import { AdminAccountTable } from "./Table/AdminAccountTable";
import { AddAdminAccountModal } from "./featureComponents/AddAdminAccountModal";
import { AccountFilterBar } from "../../uiForAll/AccountFilterBar";
import { MANAGER_ROLES, RoleDescriptions } from "../../../../constants/auth";
import { UpdateAccountModal } from "./featureComponents/UpdateAccountModal";

export const AdminAccountManagement: React.FC<{
  qlHeThong: ReturnType<typeof useQLHeThong>;
}> = ({ qlHeThong }) => {
  const {
    ui,
    loading,
    errors,
    actions,
    handlers,
    adminAccountData,
    lockHandlers,
    // 🔥 Đảm bảo hook trả về hàm này
  } = qlHeThong;

  const [showModal, setShowModal] = useState(false);

  // 🔥 State cho update
  const [showUpdateModal, setShowUpdateModal] = useState(false);
  const [selectedAcc, setSelectedAcc] = useState<any | null>(null);

  const roleOptions = [
    // Bao gồm cả quản trị hệ thống + nhóm quản lý
    { value: "QuanTriHeThong", label: RoleDescriptions["QuanTriHeThong"] },
    ...MANAGER_ROLES.map((r) => ({
      value: r,
      label: RoleDescriptions[r] || r,
    })),
  ];
  // Stats cho tài khoản quản trị
  const totalAccounts = adminAccountData.length;
  const filteredAccounts = ui.filteredAdminAccount.length;

  const statsCards = [
    StatsCardHelpers.totalCard(totalAccounts, "tài khoản"),
    StatsCardHelpers.filteredCard(filteredAccounts),
    StatsCardHelpers.activeCard(
      ui.filteredAdminAccount.filter((acc) => !acc.daBiKhoa).length
    ),
    StatsCardHelpers.lockedCard(
      ui.filteredAdminAccount.filter((acc) => acc.daBiKhoa).length
    ),
  ];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <h2 className="text-xl font-semibold text-gray-900">
          🔐 Quản lý tài khoản quản trị
        </h2>
        <button
          onClick={() => setShowModal(true)}
          className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
        >
          Thêm tài khoản
        </button>
      </div>

      {/* Stats Cards */}
      <StatsCards cards={statsCards} gridCols={4} />

      {/* Error Alert */}
      {errors.adminAccount && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4">
          <div className="flex items-center">
            <div className="text-red-500 text-xl mr-3">⚠️</div>
            <div>
              <h3 className="text-red-800 font-semibold mb-1">Có lỗi xảy ra</h3>
              <p className="text-red-700">{errors.adminAccount}</p>
            </div>
            <button
              onClick={actions.clearErrors}
              className="ml-auto text-red-400 hover:text-red-600"
            >
              Đóng
            </button>
          </div>
        </div>
      )}
      {/* FIlter Bar */}
      <AccountFilterBar
        colorTheme="blue"
        searchPlaceholder="Tìm kiếm tài khoản quản trị..."
        searchValue={ui.searchQuery}
        onSearchChange={actions.setSearchQuery}
        showRoleFilter
        roleValue={ui.filters.loaiTaiKhoan || ""}
        roleOptions={roleOptions}
        onRoleChange={actions.setRoleFilter}
        showStatusFilter
        statusValue={ui.filters.trangThai || ""}
        onStatusChange={actions.setStatusFilter}
        onRefresh={actions.loadAdminAccount}
        refreshing={loading.adminAccount}
        onClearAll={actions.clearAllFilters}
      />
      {/* Admin Account Table */}
      <AdminAccountTable
        data={ui.filteredAdminAccount}
        loading={loading.adminAccount}
        onLockToggle={lockHandlers.adminAccount.lockToggle}
        onDelete={handlers.deleteAdminAccount}
        onUpdate={(row) => {
          setSelectedAcc(row);
          setShowUpdateModal(true);
        }}
      />

      {/* Add Admin Account Modal */}
      <AddAdminAccountModal
        visible={showModal}
        onCancel={() => setShowModal(false)}
        onSuccess={() => {
          setShowModal(false);
          actions.loadAdminAccount();
        }}
      />

      {/* Update Admin Account Modal */}
      <UpdateAccountModal
        open={showUpdateModal}
        onClose={() => {
          setShowUpdateModal(false);
          setSelectedAcc(null);
        }}
        defaultValues={{
          maTaiKhoan: selectedAcc?.maTaiKhoan || "",
          userName: selectedAcc?.userName || selectedAcc?.email || "",
          loaiTaiKhoan: selectedAcc?.loaiTaiKhoan || "",
        }}
        onSuccess={() => {
          actions.loadAdminAccount();
        }}
      />
    </div>
  );
};
