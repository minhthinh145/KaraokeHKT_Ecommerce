import React, { useEffect, useMemo, useCallback } from "react";
import dayjs from "dayjs";
import { WeekNavigator } from "../../uiForAll/WeekNavigator";
import { LichLamViecTable } from "../../uiForAll/LichLamViecTable";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu";
import { mapLoaiNhanVienToLabel } from "../../../../api/types/admins/QLNhanSutypes";
import type { LichLamViecDTO } from "../../../../api";

interface LichLamViecChuyenCaTableProps {
  weekStart: string;
  onChangeWeekStart: (ws: string) => void;
}

export const LichLamViecChuyenCaTable: React.FC<
  LichLamViecChuyenCaTableProps
> = ({ weekStart, onChangeWeekStart }) => {
  const {
    caLamViecList,
    refreshCaLamViec,
    lichLamViecData,
    pheDuyetLists,
    nhanVienData,
  } = useQLNhanSu();

  useEffect(() => {
    if (!caLamViecList.length) refreshCaLamViec();
  }, [caLamViecList.length, refreshCaLamViec]);

  const caList = useMemo(
    () =>
      (caLamViecList || [])
        .slice()
        .sort((a, b) => a.maCa - b.maCa)
        .map((c) => ({
          maCa: c.maCa,
          tenCaLamViec: c.tenCa,
          gioBatDauCa: c.gioBatDauCa,
          gioKetThucCa: c.gioKetThucCa,
        })),
    [caLamViecList]
  );

  const weekDates = useMemo(
    () =>
      new Set(
        Array.from({ length: 7 }, (_, i) =>
          dayjs(weekStart).add(i, "day").format("YYYY-MM-DD")
        )
      ),
    [weekStart]
  );

  // Filter schedules theo tuần
  const scheduleWeek = useMemo(
    () =>
      (lichLamViecData || []).filter((x: any) =>
        weekDates.has(dayjs(x.ngayLamViec).format("YYYY-MM-DD"))
      ),
    [lichLamViecData, weekDates]
  );

  // Pending shift changes để vẽ line
  const pendingShiftChanges = useMemo(
    () =>
      (pheDuyetLists.pending || []).map((x: any) => ({
        maLichLamViecGoc: x.maLichLamViecGoc,
        ngayLamViecMoi: x.ngayLamViecMoi,
        maCaMoi: x.maCaMoi,
      })),
    [pheDuyetLists.pending]
  );

  // Lấy tên nhân viên
  const getNhanVienName = useCallback(
    (item: LichLamViecDTO) => {
      const nv = (nhanVienData || []).find(
        (n: any) => (n.maNhanVien ?? n.maNv) === item.maNhanVien
      );
      return nv?.hoTen || item.tenNhanVien || "";
    },
    [nhanVienData]
  );

  // Lấy vai trò nhân viên
  const getNhanVienInfo = useCallback(
    (item: LichLamViecDTO) => {
      const nv = (nhanVienData || []).find(
        (n: any) => (n.maNhanVien ?? n.maNv) === item.maNhanVien
      );
      return (
        mapLoaiNhanVienToLabel(nv?.loaiNhanVien ?? item.loaiNhanVien) || ""
      );
    },
    [nhanVienData]
  );

  return (
    <div className="space-y-3">
      <WeekNavigator
        weekStart={weekStart}
        onChangeWeekStart={onChangeWeekStart}
        className="mb-1"
        yearRange={{ min: dayjs().year() - 2, max: dayjs().year() + 2 }}
      />
      <LichLamViecTable
        caList={caList}
        weekStart={weekStart}
        data={scheduleWeek}
        canAdd={false}
        canEdit={false}
        canDelete={false}
        showSendNoti={false}
        enableShiftRequestDrag={false}
        pendingShiftChanges={pendingShiftChanges}
        getNhanVienName={getNhanVienName}
        getNhanVienInfo={getNhanVienInfo}
      />
    </div>
  );
};
