import React, { useState } from "react";
import { Typography, Input, Button, Space, Popconfirm, Tag } from "antd";
import { LichLamViecTable } from "../../admins/uiForAll/LichLamViecTable";
import { GenericQLTable } from "../../admins/uiForAll/GenericQLTable";
import { ConfirmDialog } from "../../../components/admins/uiForAll/ConfirmDialog";
import type {
  LichLamViecDTO,
  YeuCauChuyenCaDTO,
} from "../../../api/types/admins/QLNhanSutypes";
import type { CaBrief } from "../../admins/uiForAll/LichLamViecTable";
import { WeekNavigator } from "../../admins/uiForAll/WeekNavigator";
import dayjs from "dayjs";
import isBetween from "dayjs/plugin/isBetween";

dayjs.extend(isBetween);
import "dayjs/locale/vi";
dayjs.locale("vi");

interface ChuyenCaManagementProps {
  caList: CaBrief[];
  weekStart: string;
  schedule: LichLamViecDTO[];
  shiftChanges: YeuCauChuyenCaDTO[];
  shiftChangesLoading?: boolean;
  scheduleLoading?: boolean;
  draggingId: number | null;
  beginDrag: (id: number) => void;
  endDrag: () => void;
  createShiftChange: (dto: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
    lyDoChuyenCa: string;
  }) => Promise<{ success: boolean }>;
  refreshAll: () => void;
  deleteShiftChange: (id: number) => Promise<{ success: boolean }>;
  onChangeWeekStart?: (newWeekStart: string) => void;
}

export const ChuyenCaManagement: React.FC<ChuyenCaManagementProps> = ({
  caList,
  weekStart,
  schedule,
  shiftChanges,
  shiftChangesLoading,
  scheduleLoading,
  draggingId,
  beginDrag,
  endDrag,
  createShiftChange,
  refreshAll,
  deleteShiftChange,
  onChangeWeekStart,
}) => {
  const [pendingDrop, setPendingDrop] = useState<{
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  } | null>(null);
  const [confirmVisible, setConfirmVisible] = useState(false);
  const [creating, setCreating] = useState(false);
  const [lyDo, setLyDo] = useState("");

  const handleDrop = (p: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }) => {
    setPendingDrop(p);
    setConfirmVisible(true);
  };

  // Khi xác nhận:
  const handleConfirm = async () => {
    if (!pendingDrop) return;
    setCreating(true);
    const r = await createShiftChange({
      ...pendingDrop,
      lyDoChuyenCa: lyDo, // truyền lý do
    });
    setCreating(false);
    if (r.success) {
      refreshAll();
    }
    setConfirmVisible(false);
    setPendingDrop(null);
    setLyDo(""); // reset lý do
  };

  // Tách hàm xóa (để dùng trong column actions)
  const handleDelete = async (id: number) => {
    const r = await deleteShiftChange(id);
    if (r.success) refreshAll();
  };

  // Columns (thêm Ghi chú + tự xử lý Thao tác theo daPheDuyet)
  const shiftColumns = React.useMemo(
    () => [
      {
        key: "lichGoc",
        title: "Lịch gốc",
        align: "center" as const,
        render: (_: any, r: YeuCauChuyenCaDTO) => (
          <div>
            <div className="font-semibold">{r.tenCaGoc}</div>
            <div className="text-xs text-gray-500 font-semibold">
              {/* Hiển thị thứ trước ngày */}
              {`${dayjs(r.ngayLamViecGoc)
                .format("dddd")
                .replace(/^./, (c) => c.toUpperCase())} - ${dayjs(
                r.ngayLamViecGoc
              ).format("DD/MM/YYYY")}`}
            </div>
          </div>
        ),
      },
      {
        key: "lichMoi",
        title: "Lịch mới",
        align: "center" as const,
        render: (_: any, r: YeuCauChuyenCaDTO) => (
          <div>
            <div className="font-semibold">{r.tenCaMoi}</div>
            <div className="text-xs text-gray-500 font-semibold">
              {`${dayjs(r.ngayLamViecMoi)
                .format("dddd")
                .replace(/^./, (c) => c.toUpperCase())} - ${dayjs(
                r.ngayLamViecMoi
              ).format("DD/MM/YYYY")}`}
            </div>
          </div>
        ),
      },
      {
        key: "trangThaiPheDuyet",
        title: "Trạng thái phê duyệt",
        align: "center" as const,
        render: (_: any, r: YeuCauChuyenCaDTO) =>
          r.daPheDuyet ? (
            <Tag color="green">Đã duyệt</Tag>
          ) : (
            <Tag color="orange">Chờ duyệt</Tag>
          ),
      },
      {
        key: "ketQuaPheDuyet",
        title: "Kết quả phê duyệt",
        align: "center" as const,
        render: (_: any, r: YeuCauChuyenCaDTO) => {
          if (
            !r.daPheDuyet ||
            r.ketQuaPheDuyet === null ||
            r.ketQuaPheDuyet === undefined
          )
            return <span className="text-gray-500">Chưa xử lý</span>;
          if (r.ketQuaPheDuyet === true) return <Tag color="green">Đồng ý</Tag>;
          if (r.ketQuaPheDuyet === false) return <Tag color="red">Từ chối</Tag>;
          return <span className="text-gray-500">Chưa xử lý</span>;
        },
      },
      {
        key: "ghiChu",
        title: "Ghi chú",
        align: "center" as const,
        render: (_: any, r: YeuCauChuyenCaDTO) =>
          r.daPheDuyet ? (
            r.ghiChuPheDuyet ? (
              <span>{r.ghiChuPheDuyet}</span>
            ) : (
              <span className="text-gray-400 italic">Không có ghi chú</span>
            )
          ) : (
            <span className="text-gray-400 italic">Đang chờ phê duyệt</span>
          ),
      },
      {
        key: "actions",
        title: "Thao tác",
        align: "center" as const,
        width: 140,
        render: (_: any, r: YeuCauChuyenCaDTO) =>
          r.daPheDuyet ? (
            <span className="text-xs font-semibold text-gray-500">
              Đã duyệt
            </span>
          ) : (
            <Space>
              <Popconfirm
                title="Xóa yêu cầu?"
                description="Bạn chắc chắn muốn xóa yêu cầu này?"
                okText="Xóa"
                cancelText="Hủy"
                onConfirm={() => handleDelete(r.maYeuCau)}
              >
                <Button danger size="small">
                  Xóa
                </Button>
              </Popconfirm>
            </Space>
          ),
      },
    ],
    [deleteShiftChange, refreshAll]
  );

  const pendingShiftPairs = React.useMemo(
    () =>
      shiftChanges
        .filter((x) => !x.daPheDuyet)
        .map((x) => ({
          maLichLamViecGoc: x.maLichLamViecGoc,
          ngayLamViecMoi: x.ngayLamViecMoi,
          maCaMoi: x.maCaMoi,
        })),
    [shiftChanges]
  );

  // Tính ngày đầu và cuối tuần
  const weekStartDay = dayjs(weekStart).startOf("day");
  const weekEndDay = weekStartDay.add(6, "day").endOf("day");

  // Lọc các yêu cầu chưa phê duyệt ngoài tuần hiện tại
  const pendingOtherWeeks = React.useMemo(() => {
    return shiftChanges
      .filter(
        (x) =>
          !x.daPheDuyet &&
          (dayjs(x.ngayLamViecGoc).isBefore(weekStartDay) ||
            dayjs(x.ngayLamViecGoc).isAfter(weekEndDay))
      )
      .map((x) => ({
        ngay: x.ngayLamViecGoc,
      }));
  }, [shiftChanges, weekStart]);

  // Lọc shiftChanges theo tuần
  const shiftChangesOfWeek = React.useMemo(
    () =>
      shiftChanges.filter((x) => {
        const ngayGoc = dayjs(x.ngayLamViecGoc);
        const ngayMoi = dayjs(x.ngayLamViecMoi);
        return (
          (ngayGoc.isSameOrAfter(weekStartDay) &&
            ngayGoc.isSameOrBefore(weekEndDay)) ||
          (ngayMoi.isSameOrAfter(weekStartDay) &&
            ngayMoi.isSameOrBefore(weekEndDay))
        );
      }),
    [shiftChanges, weekStart]
  );

  return (
    <div>
    
      <div className="mb-4">
        <Typography.Text type="secondary">
          Kéo thả ca để tạo yêu cầu chuyển ca – chờ phê duyệt
        </Typography.Text>
      </div>

      <WeekNavigator
        weekStart={weekStart}
        onChangeWeekStart={(ws) => onChangeWeekStart?.(ws)}
        className="mb-4"
        yearRange={{ min: dayjs().year() - 1, max: dayjs().year() + 1 }}
      />

      <div className="space-y-6">
        <LichLamViecTable
          caList={caList}
          weekStart={weekStart}
          data={schedule}
          canAdd={false}
          canEdit={false}
          canDelete={false}
          showSendNoti={false}
          enableShiftRequestDrag
          draggingLichId={draggingId}
          onShiftDragStart={beginDrag}
          onShiftDragEnd={endDrag}
          onShiftDropCreate={handleDrop}
          showShiftRequestHint
          loading={scheduleLoading}
          pendingShiftChanges={pendingShiftPairs}
        />

        <GenericQLTable<YeuCauChuyenCaDTO>
          data={shiftChangesOfWeek}
          loading={!!shiftChangesLoading}
          columns={shiftColumns}
          rowKey="maYeuCau"
          // showDeleteAction removed: tự xử lý trong column actions để ẩn hiện theo daPheDuyet
          tableName="yêu cầu"
          emptyMessage="Không có yêu cầu chuyển ca"
        />

        {/* ConfirmDialog: đổi prop 'open' -> 'visible' theo type gốc */}
        <ConfirmDialog
          isOpen={confirmVisible}
          title="Tạo yêu cầu chuyển ca?"
          message={
            pendingDrop ? (
              <div>
                <div>
                  Bạn muốn chuyển sang{" "}
                  <b>
                    {caList.find((c) => c.maCa === pendingDrop.maCaMoi)
                      ?.tenCaLamViec || pendingDrop.maCaMoi}
                  </b>{" "}
                  - {dayjs(pendingDrop.ngayLamViecMoi).format("DD/MM/YYYY")}?
                </div>
                <div className="mt-2">
                  <Input.TextArea
                    placeholder="Nhập lý do chuyển ca (bắt buộc)"
                    value={lyDo}
                    onChange={(e) => setLyDo(e.target.value)}
                    autoSize={{ minRows: 2, maxRows: 4 }}
                    required
                  />
                </div>
              </div>
            ) : (
              ""
            )
          }
          onCancel={() => {
            setConfirmVisible(false);
            setPendingDrop(null);
            setLyDo("");
          }}
          onConfirm={handleConfirm}
          confirmText={creating ? "Đang tạo..." : "Tạo"}
          cancelText="Hủy"
          confirmButtonVariant="primary"
          loading={creating}
        />
      </div>
    </div>
  );
};
