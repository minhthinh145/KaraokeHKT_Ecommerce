import React, { useEffect, useCallback, useState } from "react";
import { useTienLuong } from "../../../../hooks/QLNhanSu/useTienLuong";
import { useCaLamViec } from "../../../../hooks/QLNhanSu/useCaLamViec";
import { AccountFilterBar } from "../../uiForAll/AccountFilterBar";
import { DefaultLuongCards } from "./feature/DefaultLuongCards";
import { TienLuongTable } from "./Table/TienLuongTable";
import { AddLuongModal } from "./feature/AddLuongModal";
import type { LuongCaLamViecDTO } from "../../../../api";
import { EditLuongModal } from "./feature/EditLuongModal";

export const TienLuongManagement: React.FC = () => {
  const {
    filteredTienLuong,
    defaultLuongCardsData,
    tienLuongData,
    loading,
    error,
    ui,
    refreshTienLuongData,
    addTienLuong,
    removeTienLuong,
    setSearch,
    setDateRangeFilter,
    openAdd,
    closeAdd,
    openEdit,
    closeEdit,
    updateTienLuong,
    clearFilters,
  } = useTienLuong({ autoLoad: true });

  const {
    caLamViecList,
    loading: loadingCa,
    refreshCaLamViec,
  } = useCaLamViec();

  // Load ca làm việc
  useEffect(() => {
    if (!caLamViecList.length) refreshCaLamViec();
    if (!tienLuongData.length) refreshTienLuongData();
  }, [
    caLamViecList.length,
    tienLuongData.length,
    refreshCaLamViec,
    refreshTienLuongData,
  ]);

  const handleDeleteTienLuong = useCallback(
    async (id: string) => {
      const numId = typeof id === "string" ? parseInt(id, 10) : id;
      if (isNaN(numId)) return { success: false };
      return await removeTienLuong(numId);
    },
    [removeTienLuong]
  );

  const handleEdit = (tienLuong: LuongCaLamViecDTO) => {
    openEdit(tienLuong);
  };

  const handleSubmitAdd = useCallback(
    async (form: any) => {
      await addTienLuong(form);
      closeAdd();
    },
    [addTienLuong, closeAdd]
  );

  return (
    <div className="space-y-5">
      {/* Default Luong Cards */}
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            Quản lý tiền lương
          </h1>
          <p className="mt-1 text-sm text-gray-600">
            Quản lý thông tin lương trong hệ thống
          </p>
        </div>
        <button
          onClick={openAdd}
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
          Thêm lương cho ngày lễ
        </button>
      </div>
      <DefaultLuongCards
        defaults={defaultLuongCardsData.map((d) => {
          const ca = caLamViecList.find((c) => c.maCa === d.maCa);
          return {
            ...d,
            tenCaLamViec: ca?.tenCa || d.tenCaLamViec || "",
            gioBatDauCa: ca?.gioBatDauCa || "",
            gioKetThucCa: ca?.gioKetThucCa || "",
            dto: d,
          };
        })}
        onEdit={(item) => console.log("Edit:", item)}
      />
      {/* Filter Bar */}
      <AccountFilterBar
        searchPlaceholder="Tìm kiếm theo ca hoặc lương..."
        searchValue={ui.searchQuery}
        onSearchChange={setSearch}
        showRoleFilter={false}
        showStatusFilter={false}
        onRefresh={refreshTienLuongData}
        onClearAll={clearFilters}
        refreshing={loading || loadingCa}
        showDateRangeFilter
        dateRangeValue={ui.filters.dateRange}
        onDateRangeChange={setDateRangeFilter}
        dateRangePlaceholder={["Từ ngày", "Đến ngày"]}
      />
      {/* Error Display */}
      {error && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4">
          <p className="text-red-700">{error}</p>
        </div>
      )}
      {/* Table */}
      <TienLuongTable
        data={filteredTienLuong}
        loading={loading}
        onDelete={handleDeleteTienLuong}
        onUpdate={handleEdit}
      />{" "}
      <EditLuongModal
        open={ui.showEditModal}
        onClose={closeEdit}
        onSuccess={refreshTienLuongData}
        luong={ui.selectedTienLuong ?? null}
      />
      {/* Add Modal */}
      {ui.showAddModal && (
        <AddLuongModal
          open={ui.showAddModal}
          onClose={closeAdd}
          onSubmit={handleSubmitAdd}
        />
      )}
    </div>
  );
};
