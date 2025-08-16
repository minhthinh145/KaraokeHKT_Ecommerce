import React, { useEffect, useMemo } from "react";
import { useVatLieu } from "../../../../hooks/QLKho/useVatLieu";
import { CreateVatLieuModal } from "./Modals/CreateVatLieuModal";
import { UpdateSoLuongModal } from "./Modals/UpdateSoLuongModal";
import { EditVatLieuModal } from "./Modals/EditVatLieuModal";
import { VatLieuTabView } from "./VatLieuTabView";
import { AccountFilterBar } from "../../uiForAll/AccountFilterBar";
import { useDispatch, useSelector } from "react-redux";
import { setVatLieuSearchQuery } from "../../../../redux/admin/QLKho/vatLieu/slice";
import {
  selectVatLieuUI,
  selectVatLieuItems,
  selectVatLieuLoading,
} from "../../../../redux/admin/QLKho/vatLieu/selectors";

export const QLVatLieuManagement: React.FC = () => {
  const {
    load,
    openCreate,
    closeCreate,
    closeUpdateSL,
    openEdit,
    closeEdit,
    create,
    updateSoLuong,
    toggleNgungCungCap,
    update,
  } = useVatLieu();
  const dispatch = useDispatch();
  const ui = useSelector(selectVatLieuUI);
  const dataAll = useSelector(selectVatLieuItems);
  const loading = useSelector(selectVatLieuLoading);

  // lọc theo tên (tenVatLieu hoặc tenSanPham nếu muốn)
  const filteredData = useMemo(
    () =>
      !ui.searchQuery
        ? dataAll
        : dataAll.filter(
            (x) =>
              x.tenVatLieu
                ?.toLowerCase()
                .includes(ui.searchQuery.toLowerCase()) ||
              x.tenSanPham?.toLowerCase().includes(ui.searchQuery.toLowerCase())
          ),
    [dataAll, ui.searchQuery]
  );

  useEffect(() => {
    load();
  }, [load]);

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Quản lý vật tư</h1>
          <p className="mt-1 text-sm text-gray-600">
            Quản lý thông tin vật tư trong hệ thống
          </p>
        </div>
        <button
          onClick={openCreate}
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
          Thêm vật tư
        </button>
      </div>

      {/* Filter: chỉ tìm kiếm + làm mới */}
      <AccountFilterBar
        colorTheme="green"
        searchPlaceholder="Tìm kiếm vật tư..."
        searchValue={ui.searchQuery}
        onSearchChange={(v) => dispatch(setVatLieuSearchQuery(v))}
        onRefresh={load}
        refreshing={loading}
        // các phần khác không truyền => ẩn
      />

      {/* Bảng / Tabs */}
      <VatLieuTabView
        data={filteredData}
        loading={loading}
        onUpdate={openEdit}
        onToggleNgungCungCap={(row, next) =>
          toggleNgungCungCap(row.maVatLieu, next)
        }
      />

      {/* Modals */}
      <CreateVatLieuModal
        open={ui.showCreateModal}
        onClose={closeCreate}
        onSubmit={async (payload) => {
          const ok = await create(payload);
          if (ok?.success) closeCreate();
        }}
      />

      <UpdateSoLuongModal
        open={ui.showUpdateSoLuongModal}
        row={ui.selected}
        onClose={closeUpdateSL}
        onSubmit={async (maVatLieu, soLuongMoi) => {
          const ok = await updateSoLuong(maVatLieu, soLuongMoi);
          if (ok?.success) closeUpdateSL();
        }}
      />

      <EditVatLieuModal
        open={ui.showEditModal}
        row={ui.selected}
        onClose={closeEdit}
        onSubmit={async (payload) => {
          const ok = await update(payload);
          if (ok?.success) closeEdit();
        }}
      />
    </div>
  );
};
