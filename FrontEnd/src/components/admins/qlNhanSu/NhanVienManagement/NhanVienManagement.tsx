// filepath: d:\OOAD_Project\FrontEnd\src\components\admins\qlNhanSu\NhanVienManagement\NhanVienManagement.tsx
import React, { useEffect } from "react";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu";
import { NhanVienTable } from "./Table/NhanVienTable";
import { AccountFilterBar } from "../../uiForAll/AccountFilterBar";
import { AddNhanVienModal } from "./feature/AddNhanVienModal";
import { EditNhanVienModal } from "./feature/EditNhanVienModal";
import { StatsCards, StatsCardHelpers } from "../../uiForAll/StatsCards";
import type { NhanVienDTO } from "../../../../api/types/admins/QLNhanSutypes";

// Role options cho nhân viên
const NHAN_VIEN_ROLE_OPTIONS = [
  { value: "", label: "Tất cả vai trò" },
  { value: "NhanVienKho", label: "Nhân viên kho" },
  { value: "NhanVienPhucVu", label: "Nhân viên phục vụ" },
  { value: "NhanVienTiepTan", label: "Nhân viên tiếp tân" },
];

export const NhanVienManagement: React.FC = () => {
  const {
    filteredNhanVien: data,
    nhanVienStats,
    nhanVienLoading: loading,
    nhanVienError,
    nhanVienUI,
    nhanVienActions,
    nhanVienHandlers,
  } = useQLNhanSu();

  useEffect(() => {
    nhanVienActions.load();
  }, []);

  const handleEdit = (nhanVien: NhanVienDTO) => {
    nhanVienActions.openEditModal(nhanVien);
  };

  const handleNghiViec = (nhanVien: NhanVienDTO, value: boolean) => {
    nhanVienHandlers.updateDaNghiViec(nhanVien.maNv, value);
  };
  // ✨ Stats cards data với helper functions
  const statsCards = [
    StatsCardHelpers.totalNhanVienCard(nhanVienStats.total),
    StatsCardHelpers.nhanVienKhoCard(nhanVienStats.byType.NhanVienKho || 0),
    StatsCardHelpers.nhanVienPhucVuCard(
      nhanVienStats.byType.NhanVienPhucVu || 0
    ),
    StatsCardHelpers.nhanVienTiepTanCard(
      nhanVienStats.byType.NhanVienTiepTan || 0
    ),
  ];

  // Tách 2 danh sách
  const nhanVienDangLam = React.useMemo(
    () => data.filter((x) => !x.daNghiViec),
    [data]
  );
  const nhanVienDaNghi = React.useMemo(
    () => data.filter((x) => x.daNghiViec),
    [data]
  );

  return (
    <div className="space-y-6">
      {/* Header & Stats */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Quản lý nhân sự</h1>
          <p className="mt-1 text-sm text-gray-600">
            Quản lý thông tin nhân viên trong hệ thống
          </p>
        </div>
        <button
          onClick={nhanVienActions.openAddModal}
          className="inline-flex items-center px-4 py-2 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
        >
          <svg
            className="w-4 h-4 mr-2"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M12 6v6m0 0v6m0-6h6m-6 0H6"
            />
          </svg>
          Thêm nhân viên
        </button>
      </div>

      {/* Stats Cards */}
      <StatsCards cards={statsCards} gridCols={4} />

      {/* Error Display */}
      {nhanVienError && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4">
          <div className="flex">
            <svg
              className="w-5 h-5 text-red-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
            <div className="ml-3">
              <h3 className="text-sm font-medium text-red-800">
                Có lỗi xảy ra
              </h3>
              <p className="text-sm text-red-700 mt-1">{nhanVienError}</p>
            </div>
          </div>
        </div>
      )}

      {/* Filter Bar */}
      <AccountFilterBar
        colorTheme="green"
        searchPlaceholder="Tìm kiếm nhân viên theo tên, email, SĐT..."
        searchValue={nhanVienUI.searchQuery}
        onSearchChange={nhanVienActions.setSearchQuery}
        showRoleFilter
        roleValue={nhanVienUI.filters.loaiNhanVien || ""}
        roleOptions={NHAN_VIEN_ROLE_OPTIONS}
        onRoleChange={nhanVienActions.setRoleFilter}
        onRefresh={nhanVienActions.load}
        refreshing={loading}
        onClearAll={nhanVienActions.clearAllFilters}
      />

      {/* Bảng Nhân viên đang làm việc: có Sửa + Cho nghỉ việc */}
      <div className="space-y-2">
        <h3 className="text-base font-semibold text-gray-900">
          Nhân viên đang làm việc ({nhanVienDangLam.length})
        </h3>
        <NhanVienTable
          data={nhanVienDangLam}
          loading={loading}
          onUpdate={handleEdit}
          onToggleNghiViec={(nv, next) => handleNghiViec(nv, next)}
          showUpdateAction={true}
          showNghiViecAction={true}
        />
      </div>

      {/* Bảng Nhân viên đã nghỉ việc: chỉ Trở lại làm việc, KHÔNG sửa */}
      <div className="space-y-2">
        <h3 className="text-base font-semibold text-gray-900">
          Nhân viên đã nghỉ việc ({nhanVienDaNghi.length})
        </h3>
        <NhanVienTable
          data={nhanVienDaNghi}
          loading={loading}
          onUpdate={handleEdit} // sẽ không hiện vì showUpdateAction=false
          onToggleNghiViec={(nv) => handleNghiViec(nv, false)}
          showUpdateAction={false}
          showNghiViecAction={true}
        />
      </div>

      <AddNhanVienModal
        isOpen={nhanVienUI.showAddModal}
        onClose={nhanVienActions.closeAddModal}
        onSuccess={() => {
          nhanVienActions.closeAddModal();
          nhanVienActions.load();
        }}
        roleOptions={NHAN_VIEN_ROLE_OPTIONS}
      />

      <EditNhanVienModal
        isOpen={nhanVienUI.showEditModal}
        onClose={nhanVienActions.closeEditModal}
        nhanVien={nhanVienUI.selectedNhanVien ?? null}
        onSuccess={() => {
          nhanVienActions.closeEditModal();
          nhanVienActions.load();
        }}
      />
    </div>
  );
};
