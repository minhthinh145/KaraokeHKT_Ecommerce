import React, { useMemo } from "react";
import { GenericQLTable } from "../../../uiForAll/GenericQLTable";
import { useCaLamViec } from "../../../../../hooks/QLNhanSu/useCaLamViec";
import type { LuongCaLamViecDTO } from "../../../../../api";

interface Props {
  data: LuongCaLamViecDTO[];
  loading: boolean;
  onDelete: (id: string) => Promise<{ success: boolean }>;
  onUpdate: (luongCaLam: LuongCaLamViecDTO) => void;
}

export const TienLuongTable: React.FC<Props> = ({
  data,
  loading,
  onDelete,
  onUpdate,
}) => {
  const { caMap } = useCaLamViec(); // Lấy caMap từ hook

  // Chỉ dữ liệu đặc biệt (isDefault=false)
  const specialData = useMemo(() => data.filter((d) => !d.isDefault), [data]);

  const columns = useMemo(
    () => [
      {
        key: "tenCaLamViec",
        title: "Ca",
        render: (_: any, r: LuongCaLamViecDTO) =>
          caMap[r.maCa] || r.tenCaLamViec || "Không xác định", // Dùng caMap
      },
      {
        key: "luong",
        title: "Lương ca (VNĐ)",
        render: (_: any, r: LuongCaLamViecDTO) =>
          r.giaCa.toLocaleString("vi-VN") + " ₫",
      },
      {
        key: "ngayApDung",
        title: "Ngày áp dụng",
        render: (_: any, r: LuongCaLamViecDTO) =>
          r.ngayApDung
            ? new Date(r.ngayApDung).toLocaleDateString("vi-VN")
            : "-",
      },
      {
        key: "ngayKetThuc",
        title: "Ngày kết thúc",
        render: (_: any, r: LuongCaLamViecDTO) =>
          r.ngayKetThuc
            ? new Date(r.ngayKetThuc).toLocaleDateString("vi-VN")
            : "-",
      },
    ],
    [caMap, onDelete]
  );

  return (
    <GenericQLTable<LuongCaLamViecDTO>
      data={specialData}
      loading={loading}
      columns={columns as any}
      showDeleteAction={true}
      showUpdateAction={true}
      onDelete={onDelete}
      onUpdate={onUpdate}
      rowKey="maLuongCaLamViec"
      emptyMessage="Không có lương ngày đặc biệt"
      titleDeleteAction="Xóa lương ngày đặc biệt"
      messageDeleteAction="Bạn có chắc chắn muốn xóa lương ngày đặc biệt này không? Hành động này sẽ không thể hoàn tác."
    />
  );
};
