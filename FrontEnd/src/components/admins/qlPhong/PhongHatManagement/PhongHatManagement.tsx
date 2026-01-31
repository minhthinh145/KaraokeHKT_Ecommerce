import React, { useMemo } from "react";
import { Input, Segmented } from "antd";
import { useQLPhong } from "../../../../hooks/QLPhong/useQLPhong";
import { PhongHatTable } from "./Table/PhongHatTable";
import { CreatePhongHatModal } from "./Modals/CreatePhongHatModal";
import { EditPhongHatModal } from "./Modals/EditPhongHatModal";

export const PhongHatManagement: React.FC = () => {
  const { phongHat, loaiPhong } = useQLPhong();
  const {
    filtered,
    ui,
    loading,
    setSearch,
    setStatusFilter,
    openAdd,
    closeAdd,
    openEdit,
    closeEdit,
    add,
    update,
    toggleNgung,
  } = {
    filtered: phongHat.filtered,
    ui: phongHat.ui,
    loading: phongHat.loading,
    setSearch: phongHat.setSearch,
    setStatusFilter: phongHat.setStatusFilter,
    openAdd: phongHat.openAdd,
    closeAdd: phongHat.closeAdd,
    openEdit: phongHat.openEdit,
    closeEdit: phongHat.closeEdit,
    add: phongHat.add,
    update: phongHat.update,
    toggleNgung: phongHat.toggleNgung,
  };

  const activeList = useMemo(
    () => filtered.filter((f) => !f.ngungHoatDong),
    [filtered]
  );
  const inactiveList = useMemo(
    () => filtered.filter((f) => f.ngungHoatDong),
    [filtered]
  );

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between flex-wrap gap-4">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">
            Quản lý phòng hát
          </h1>
          <p className="text-sm text-slate-600 mt-1">
            Thêm / sửa / tạm ngưng hoạt động phòng
          </p>
        </div>
        <div className="flex gap-3">
          <button
            onClick={openAdd}
            className="px-4 py-2 bg-indigo-600 hover:bg-indigo-700 text-white rounded-lg text-sm font-medium"
          >
            + Thêm phòng
          </button>
        </div>
      </div>

      <div className="flex flex-wrap gap-3 items-center">
        <Input
          placeholder="Tìm theo tên / loại phòng..."
          value={ui.searchQuery}
          onChange={(e) => setSearch(e.target.value)}
          allowClear
          style={{ maxWidth: 300 }}
        />
      </div>

      <div className="space-y-8">
        <div>
          <h2 className="font-semibold text-slate-800 mb-3">Đang hoạt động</h2>
          <PhongHatTable
            data={activeList}
            loading={loading}
            onEdit={openEdit}
            onToggleNgung={(row) => toggleNgung(row, true)}
          />
        </div>
        <div>
          <h2 className="font-semibold text-slate-800 mb-3">Ngừng hoạt động</h2>
          <PhongHatTable
            data={inactiveList}
            loading={loading}
            onEdit={openEdit}
            onToggleNgung={toggleNgung}
          />
        </div>
      </div>

      <CreatePhongHatModal
        open={ui.showAddModal}
        onClose={closeAdd}
        loaiPhongOptions={loaiPhong.data}
        onSubmit={async (p) => {
          const rs = await add(p);
          if (rs.success) closeAdd();
          return rs;
        }}
      />

      <EditPhongHatModal
        open={ui.showEditModal}
        onClose={closeEdit}
        row={ui.selected}
        onSubmit={async (p) => {
          const rs = await update(p);
          if (rs.success) closeEdit();
          return rs;
        }}
        loaiPhongOptions={loaiPhong.data}
      />
    </div>
  );
};
