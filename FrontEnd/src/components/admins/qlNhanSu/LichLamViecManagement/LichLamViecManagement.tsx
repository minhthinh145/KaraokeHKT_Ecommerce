import React, { useCallback, useEffect, useMemo, useState } from "react";
import dayjs from "dayjs";
import { Typography } from "antd";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu";
import { LichLamViecTable } from "../../uiForAll/LichLamViecTable";
import {
  AddLichLamViecModal,
  type NhanVienOption,
} from "./feature/AddLichLamViecModal";
import type { LichLamViecDTO } from "../../../../api";
import { WeekNavigator } from "../../uiForAll/WeekNavigator";
import { mapLoaiNhanVienToLabel } from "../../../../api/types/admins/QLNhanSutypes";
import { EditLichLamViecModal } from "./feature/EditLichLamViecModal";
import RefreshButton from "../../../common/RefreshButton";

const { Title } = Typography;

const toMonday = (d: dayjs.Dayjs) => {
  const dow = d.day(); // 0=CN ... 6=T7
  const delta = dow === 0 ? -6 : 1 - dow; // về thứ 2
  return d.add(delta, "day").startOf("day");
};

export const LichLamViecManagement: React.FC = () => {
  const {
    caLamViecList,
    refreshCaLamViec,
    nhanVienData,
    lichLamViecData,
    filteredLichLamViec,
    lichLamViecActions,
    lichLamViecHandlers,
    lichLamViecSendingNoti,
  } = useQLNhanSu();

  const { loadByRange, setDateRange } = lichLamViecActions; // lấy ra function, không dùng object trong deps

  useEffect(() => {
    if (!caLamViecList || caLamViecList.length === 0) {
      refreshCaLamViec();
    }
  }, [caLamViecList, refreshCaLamViec]);

  // Persist tuần hiện tại (tránh bị reset khi quay lại trang)
  const [weekStart, setWeekStart] = useState<string>(() => {
    return (
      localStorage.getItem("llv.weekStart") ||
      toMonday(dayjs()).format("YYYY-MM-DD")
    );
  });
  useEffect(() => {
    localStorage.setItem("llv.weekStart", weekStart);
  }, [weekStart]);

  const weekRange = useMemo(() => {
    const start = dayjs(weekStart);
    const end = start.add(6, "day").format("YYYY-MM-DD");
    return [weekStart, end] as [string, string];
  }, [weekStart]);

  useEffect(() => {
    // gọi 1 lần khi range đổi: fetch dữ liệu tuần mới
    loadByRange(weekRange[0], weekRange[1]);
    setDateRange(weekRange);
  }, [weekRange[0], weekRange[1], loadByRange, setDateRange]);

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

  const nhanVienOptions: NhanVienOption[] = useMemo(() => {
    return (nhanVienData || []).map((nv: any) => ({
      value: nv.maNv,
      label: nv.hoTen,
      description: mapLoaiNhanVienToLabel(nv.loaiNhanVien),
    }));
  }, [nhanVienData]);

  const [modalOpen, setModalOpen] = useState(false);
  const [modalNgay, setModalNgay] = useState<string>("");
  const [modalMaCa, setModalMaCa] = useState<number>(0);

  const caTitle = useMemo(() => {
    const ca = caLamViecList?.find((c) => c.maCa === modalMaCa);
    if (!ca) return `Ca ${modalMaCa}`;
    const ten = ca.tenCa;
    return `${ten} - ${ca.gioBatDauCa} → ${ca.gioKetThucCa}`;
  }, [modalMaCa, caLamViecList]);

  const isDuplicate = useCallback(
    (maNhanVien: string) => {
      const day = modalNgay;
      return (lichLamViecData || []).some(
        (x: LichLamViecDTO) =>
          x.maCa === modalMaCa &&
          x.maNhanVien === maNhanVien &&
          dayjs(x.ngayLamViec).format("YYYY-MM-DD") === day
      );
    },
    [modalMaCa, modalNgay, lichLamViecData]
  );

  const handleAddFromCell = useCallback(
    ({ ngayLamViec, maCa }: { ngayLamViec: string; maCa: number }) => {
      setModalNgay(ngayLamViec);
      setModalMaCa(maCa);
      setModalOpen(true);
    },
    []
  );

  const handleSubmitAdd = useCallback(
    async (payload: {
      ngayLamViec: string;
      maNhanVien: string;
      maCa: number;
    }) => {
      const res = await lichLamViecHandlers.add(payload as any);
      if (res && (res as any).success) {
        setModalOpen(false);
        // reload tuần hiện tại
        lichLamViecActions.setDateRange(weekRange);
      } else {
      }
      return res as any;
    },
    [lichLamViecHandlers, lichLamViecActions, weekRange]
  );

  const getNhanVienName = useCallback(
    (item: LichLamViecDTO) => {
      const nv = (nhanVienData || []).find(
        (n: any) => (n.maNhanVien ?? n.maNv) === item.maNhanVien
      );
      return nv?.hoTen || item.tenNhanVien || "";
    },
    [nhanVienData]
  );

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

  const handleSendNoti = React.useCallback(
    ({ start, end }: { start: string; end: string }) => {
      lichLamViecActions.sendNotiRange(start, end);
    },
    [lichLamViecActions]
  );

  const handleRefresh = useCallback(() => {
    refreshCaLamViec();
    loadByRange(weekRange[0], weekRange[1]); // reload lịch tuần hiện tại
  }, [refreshCaLamViec, loadByRange, weekRange]);

  return (
    <div className="w-full">
      <div className="mb-4 flex items-start justify-between gap-4">
        <div>
          <Title level={3} className="!mb-0">
            Thêm ca làm việc
          </Title>
          <Typography.Text type="secondary">
            Quản lý lịch làm việc theo tuần
          </Typography.Text>
        </div>
        <RefreshButton onClick={handleRefresh} />
      </div>

      <WeekNavigator
        weekStart={weekStart}
        onChangeWeekStart={setWeekStart}
        className="mb-3"
        yearRange={{ min: dayjs().year() - 3, max: dayjs().year() + 3 }}
      />

      <LichLamViecTable
        caList={caList}
        weekStart={weekStart}
        data={filteredLichLamViec || []}
        canAdd
        canEdit
        canDelete
        onAdd={handleAddFromCell}
        getNhanVienName={getNhanVienName}
        getNhanVienInfo={getNhanVienInfo}
        onEdit={lichLamViecActions.openEdit}
        onDelete={(item) => lichLamViecHandlers.remove(item.maLichLamViec)}
        onSendNoti={handleSendNoti}
        sendingNoti={lichLamViecSendingNoti}
      />

      <AddLichLamViecModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        defaultNgay={modalNgay}
        defaultMaCa={modalMaCa}
        caTitle={caTitle}
        nhanVienOptions={nhanVienOptions}
        onSubmit={handleSubmitAdd}
        isDuplicate={isDuplicate}
      />

      <EditLichLamViecModal />
    </div>
  );
};
