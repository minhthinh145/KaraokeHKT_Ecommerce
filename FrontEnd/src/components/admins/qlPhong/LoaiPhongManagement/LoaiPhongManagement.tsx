import React, { useEffect, useMemo } from "react";
import { useQLPhong } from "../../../../hooks/QLPhong/useQLPhong";
import { LoaiPhongTable } from "./Table/LoaiPhongTable";
import { CreateLoaiPhongModal } from "./Modals/CreateLoaiPhongModal";
import { EditLoaiPhongModal } from "./Modals/EditLoaiPhongModal";
import { Input } from "antd";

export const LoaiPhongManagement: React.FC = () => {
  const { loaiPhong } = useQLPhong();
  const {
    data,
    loading,
    ui,
    refresh,
    add,
    update,
    openAdd,
    closeAdd,
    openEdit,
    closeEdit,
    setSearchQuery,
  } = {
    data: loaiPhong.data,
    loading: loaiPhong.loading,
    ui: loaiPhong.ui,
    refresh: loaiPhong.refresh,
    add: loaiPhong.add,
    update: loaiPhong.update,
    openAdd: loaiPhong.openAdd,
    closeAdd: loaiPhong.closeAdd,
    openEdit: loaiPhong.openEdit,
    closeEdit: loaiPhong.closeEdit,
    setSearchQuery: loaiPhong.setSearchQuery,
  };

  useEffect(() => {
    refresh();
  }, [refresh]);

  const filtered = useMemo(
    () =>
      !ui.searchQuery
        ? data
        : data.filter((x) =>
            x.tenLoaiPhong.toLowerCase().includes(ui.searchQuery.toLowerCase())
          ),
    [data, ui.searchQuery]
  );

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            Quản lý loại phòng
          </h1>
          <p className="text-sm text-gray-600 mt-1">
            Thêm / chỉnh sửa thông tin loại phòng hát
          </p>
        </div>
        <button
          onClick={openAdd}
          className="inline-flex items-center px-4 py-2 rounded-lg bg-indigo-600 text-white text-sm font-medium hover:bg-indigo-700"
        >
          + Thêm loại phòng
        </button>
      </div>

      <div className="flex gap-3">
        <Input
          placeholder="Tìm theo tên loại phòng..."
          value={ui.searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          allowClear
          style={{ maxWidth: 320 }}
        />
        <button
          className="px-3 py-2 rounded-lg bg-slate-100 hover:bg-slate-200 text-sm"
          onClick={refresh}
          disabled={loading}
        >
          Làm mới
        </button>
      </div>

      <LoaiPhongTable
        data={filtered}
        loading={loading}
        onUpdate={(row) => openEdit(row)}
      />

      <CreateLoaiPhongModal
        open={ui.showAddModal}
        onClose={closeAdd}
        onSubmit={async (p) => {
          const rs = await add(p);
          if (rs.success) closeAdd();
          return rs;
        }}
      />

      <EditLoaiPhongModal
        open={ui.showEditModal}
        onClose={closeEdit}
        row={ui.selected}
        onSubmit={async (p) => {
          const rs = await update(p);
          if (rs.success) closeEdit();
          return rs;
        }}
      />
    </div>
  );
};
