// filepath: d:\OOAD_Project\FrontEnd\src\components\admins\qlNhanSu\NhanVienManagement\NhanVienManagement.tsx
import React, { useEffect } from "react";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu"; // 🔥 Fix: Use QLNhanSu hook
import { NhanVienTable } from "./Table/NhanVienTable";
import { NhanVienFilters } from "./NhanVienFilters";
import { AddNhanVienModal } from "./AddNhanVienModal";
import { EditNhanVienModal } from "./EditNhanVienModal";
import type { NhanVienDTO } from "../../../../api"; // 🔥 Fix: Use QLNhanSu types

export const NhanVienManagement: React.FC = () => {
  const { state, ui, loading, errors, actions, handlers } = useQLNhanSu(); // 🔥 Fix: Use QLNhanSu hook

  useEffect(() => {
    actions.loadData();
  }, []);

  const handleEdit = (nhanVien: NhanVienDTO) => {
    actions.openEditModal(nhanVien);
  };

  const handleDelete = async (maNv: string) => {
    if (window.confirm("Bạn có chắc chắn muốn xóa nhân viên này?")) {
      const result = handlers.deleteNhanVien(maNv);
      if (result.success) {
      }
    }
  };

  useEffect(() => {
    return () => {
      actions.clearErrors();
    };
  }, []);

  return (
    <div className="space-y-6">
      {/* 📊 Header & Stats */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Quản lý nhân sự</h1>
          <p className="mt-1 text-sm text-gray-600">
            Quản lý thông tin nhân viên trong hệ thống
          </p>
        </div>
        <div className="flex items-center gap-3">
          <div className="text-sm text-gray-600">
            Tổng số:{" "}
            <span className="font-medium text-gray-900">
              {state.nhanVien.total}
            </span>
          </div>
          <button
            onClick={actions.openAddModal}
            className="inline-flex items-center px-4 py-2 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
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
      </div>

      {/* 🚨 Error Display */}
      {errors.nhanVien && (
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
              <p className="text-sm text-red-700 mt-1">{errors.nhanVien}</p>
            </div>
            <button
              onClick={actions.clearErrors}
              className="ml-auto text-red-400 hover:text-red-600"
            >
              <svg
                className="w-5 h-5"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M6 18L18 6M6 6l12 12"
                />
              </svg>
            </button>
          </div>
        </div>
      )}

      {/* 🔍 Filters */}
      <NhanVienFilters
        searchQuery={ui.searchQuery}
        onSearchChange={actions.search}
        loaiNhanVienFilter={ui.filters.loaiNhanVien}
        onLoaiNhanVienChange={actions.filterByType}
        filterOptions={ui.filterOptions}
        onClearFilters={actions.clearFilters}
        totalCount={state.nhanVien.total}
        filteredCount={state.nhanVien.filtered.length}
      />

      {/* 📋 Table */}
      <NhanVienTable
        data={state.nhanVien.filtered}
        loading={loading.nhanVien}
        onEdit={handleEdit}
        onDelete={handleDelete}
      />

      {/* 📝 Add Modal */}
      <AddNhanVienModal
        isOpen={ui.modals.showAdd}
        onClose={actions.closeAddModal}
        roleOptions={ui.filterOptions.filter((opt) => opt.value !== "All")}
      />

      {/* ✏️ Edit Modal */}

      <EditNhanVienModal
        isOpen={ui.modals.showEdit}
        onClose={actions.closeEditModal}
        nhanVien={ui.modals.selectedNhanVien}
        roleOptions={ui.filterOptions.filter((opt) => opt.value !== "All")}
      />
    </div>
  );
};
