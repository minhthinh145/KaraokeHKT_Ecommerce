import React, { useMemo } from "react";
import type { LichLamViecDTO } from "../../../api/types/admins/QLNhanSutypes";
import {
  LichLamViecTable,
  type CaBrief,
} from "../../admins/uiForAll/LichLamViecTable";
import { mapLoaiNhanVienToLabel } from "../../../api/types/admins/QLNhanSutypes";

interface EmployeeLichLamViecTableProps {
  caList: CaBrief[];
  weekStart: string;
  data: LichLamViecDTO[];
  loading?: boolean;
}

export const EmployeeLichLamViecTable: React.FC<
  EmployeeLichLamViecTableProps
> = ({ caList, weekStart, data, loading }) => {
  // Map info (ẩn edit/delete)
  const getNhanVienInfo = (item: LichLamViecDTO) =>
    mapLoaiNhanVienToLabel(item.loaiNhanVien);

  return (
    <LichLamViecTable
      caList={caList}
      weekStart={weekStart}
      data={data}
      canAdd={false}
      canEdit={false}
      canDelete={false}
      loading={loading}
      getNhanVienInfo={getNhanVienInfo}
      onSendNoti={undefined}
      sendingNoti={false}
    />
  );
};
