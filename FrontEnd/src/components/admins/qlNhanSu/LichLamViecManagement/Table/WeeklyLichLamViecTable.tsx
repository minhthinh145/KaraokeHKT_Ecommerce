import React from "react";
import { LichLamViecTable } from "../../../uiForAll/LichLamViecTable";
import type { LichLamViecDTO } from "../../../../../api";

export interface CaBrief {
  maCa: number;
  tenCaLamViec: string;
  gioBatDauCa: string;
  gioKetThucCa: string;
}

interface WeeklyLichLamViecTableProps {
  caList: any[];
  weekStart: string;
  data: any[];
  employeeMode?: boolean;
  onShiftDragStart?: (id: number) => void;
  onShiftDragEnd?: () => void;
  onShiftDropCreate?: (p: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }) => void;
  draggingLichId?: number | null;
  pendingShiftRequests?: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }[];
}

export const WeeklyLichLamViecTable: React.FC<WeeklyLichLamViecTableProps> = (
  props
) => {
  return <LichLamViecTable {...props} />;
};
