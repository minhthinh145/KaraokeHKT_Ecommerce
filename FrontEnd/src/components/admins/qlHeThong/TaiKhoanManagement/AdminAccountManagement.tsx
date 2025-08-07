import React, { useState } from "react";
import { useQLHeThong } from "../../../../hooks/useQLHeThong";
import { StatsCardHelpers, StatsCards } from "../../uiForAll/StatsCards";
import { AdminAccountTable } from "./Table/AdminAccountTable";

export const AdminAccountManagement: React.FC = () => {
  const { ui, loading, errors, actions, data } = useQLHeThong();
  const [showModal, setShowModal] = useState(false);

  // Stats cho tài khoản quản trị
  const totalAccounts = data.length;
  const filteredAccounts = ui.filteredAdminAccount.length;

  const statsCards = [
    StatsCardHelpers.totalCard(totalAccounts, "tài khoản"),
    StatsCardHelpers.filteredCard(filteredAccounts),
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
          className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
        >
          Thêm tài khoản
        </button>
      </div>

      {/* Stats Cards */}
      <StatsCards cards={statsCards} gridCols={2} />

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

      {/* Admin Account Table */}
      <AdminAccountTable
        data={ui.filteredAdminAccount}
        loading={loading.adminAccount}
      />
    </div>
  );
};
