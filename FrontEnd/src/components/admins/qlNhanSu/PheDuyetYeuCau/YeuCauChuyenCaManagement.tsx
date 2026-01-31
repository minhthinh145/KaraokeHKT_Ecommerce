import React, { useState, useMemo } from "react";
import dayjs from "dayjs";
import { LichLamViecChuyenCaTable } from "./LichLamViecChuyenCaTable";
import { XemYeuCauTable } from "./XemYeuCauTable";
import { Tag, Typography } from "antd";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu";
import RefreshButton from "../../../common/RefreshButton";
import "dayjs/locale/vi";
dayjs.locale("vi");

export const YeuCauChuyenCaManagement: React.FC = () => {
  // lưu tuần như trang LichLamViecManagement
  const [weekStart, setWeekStart] = useState<string>(() => {
    return (
      localStorage.getItem("pheduyet.weekStart") ||
      dayjs().startOf("week").add(1, "day").format("YYYY-MM-DD")
    );
  });

  const handleChangeWeek = (ws: string) => {
    setWeekStart(ws);
    localStorage.setItem("pheduyet.weekStart", ws);
  };

  // Lấy danh sách yêu cầu chưa phê duyệt ngoài tuần hiện tại
  const { pheDuyetLists, pheDuyetActions } = useQLNhanSu(); // thêm pheDuyetActions (nếu hook cung cấp)
  const weekStartDay = dayjs(weekStart).startOf("day");
  const weekEndDay = weekStartDay.add(6, "day").endOf("day");

  const pendingOtherWeeks = useMemo(() => {
    return (pheDuyetLists.all || [])
      .filter(
        (x) =>
          !x.daPheDuyet &&
          (dayjs(x.ngayLamViecGoc).isBefore(weekStartDay) ||
            dayjs(x.ngayLamViecGoc).isAfter(weekEndDay))
      )
      .map((x) => x.ngayLamViecGoc);
  }, [pheDuyetLists.all, weekStart]);

  const handleRefresh = () => {
    // Gọi lại load tất cả yêu cầu (tùy theo hook thực tế)
    pheDuyetActions?.loadAll?.();
  };

  return (
    <div className="space-y-8">
      <div className="flex items-center justify-between gap-3">
        <div className="flex items-center gap-3 font-bold text-xl">
          <Typography.Title level={3} className="!mb-0">
            Yêu cầu chuyển ca (tuần)
          </Typography.Title>
          {pendingOtherWeeks.length > 0 && (
            <Tag color="warning" style={{ fontWeight: 500 }}>
              Còn yêu cầu chờ phê duyệt ở&nbsp;
              {pendingOtherWeeks
                .map(
                  (ngay) =>
                    `${dayjs(ngay)
                      .format("dddd")
                      .replace(/^./, (c) => c.toUpperCase())} - ${dayjs(
                      ngay
                    ).format("DD/MM/YYYY")}`
                )
                .join(", ")}
            </Tag>
          )}
        </div>
        <RefreshButton onClick={handleRefresh} />
      </div>
      <LichLamViecChuyenCaTable
        weekStart={weekStart}
        onChangeWeekStart={handleChangeWeek}
      />
      <XemYeuCauTable weekStart={weekStart} />
    </div>
  );
};
