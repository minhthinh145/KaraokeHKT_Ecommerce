import React, { useEffect, useMemo, useState } from "react";
import dayjs from "dayjs";
import { Tag, Button, Avatar, Tabs, Progress, message } from "antd";
import { CustomerLayout } from "../../components/customer/CustomerLayout";
import { useBooking } from "../../hooks/customer/useBooking";
import { GenericQLTable } from "../../components/admins/uiForAll/GenericQLTable";
import type { LichSuDatPhongDTO } from "../../api/types";

export const BookingHistoryPage: React.FC = () => {
  const {
    history,
    unpaid,
    historyLoading,
    unpaidLoading,
    rePayLoading,
    confirmingPayment,
    paymentUrl,
    actions: { cancel, loadHistory, loadUnpaid, rePay, confirm },
  } = useBooking({ autoLoad: true });

  useEffect(() => {
    loadHistory();
    loadUnpaid();
  }, [loadHistory, loadUnpaid]);

  useEffect(() => {
    if (paymentUrl) {
      window.location.href = paymentUrl;
    }
  }, [paymentUrl]);

  // STATUS MAP
  const STATUS_INFO: Record<
    LichSuDatPhongDTO["trangThai"],
    { color: string; label: string }
  > = {
    ChuaThanhToan: { color: "orange", label: "Chưa thanh toán" },
    DaThanhToan: { color: "green", label: "Đã thanh toán" },
    DangSuDung: { color: "blue", label: "Đang sử dụng" },
    DaHoanThanh: { color: "default", label: "Đã hoàn thành" },
    DaHuy: { color: "red", label: "Đã hủy" },
    HetHanThanhToan: { color: "red", label: "Hết hạn thanh toán" },
  };

  // Columns
  const columns = useMemo(
    () => [
      {
        key: "room",
        title: "Phòng",
        width: 220,
        render: (_: any, r: LichSuDatPhongDTO) => (
          <div className="flex items-center gap-3">
            {r.hinhAnhPhong ? (
              <Avatar shape="square" src={r.hinhAnhPhong} size={48} />
            ) : (
              <Avatar shape="square" size={48}>
                {r.tenPhong?.charAt(0)}
              </Avatar>
            )}
            <div className="leading-tight">
              <div className="font-semibold text-slate-800">{r.tenPhong}</div>
              <div className="text-xs text-slate-500">
                {r.tenLoaiPhong || "—"}
              </div>
            </div>
          </div>
        ),
      },
      {
        key: "times",
        title: "Thời gian",
        width: 210,
        render: (_: any, r: LichSuDatPhongDTO) => (
          <div className="text-xs leading-5">
            <div>BĐ: {dayjs(r.thoiGianBatDau).format("DD/MM/YYYY HH:mm")}</div>
            <div>
              KT:{" "}
              {r.thoiGianKetThuc
                ? dayjs(r.thoiGianKetThuc).format("DD/MM/YYYY HH:mm")
                : "—"}
            </div>
            <div>Giờ: {r.soGioSuDung}</div>
          </div>
        ),
      },
      {
        key: "paymentWindow",
        title: "Thanh toán",
        width: 190,
        render: (_: any, r: LichSuDatPhongDTO) => {
          if (r.trangThai !== "ChuaThanhToan")
            return <span className="text-xs text-slate-500">—</span>;
          if (r.daHetHanThanhToan)
            return (
              <span className="text-xs font-medium text-red-600">Hết hạn</span>
            );
          if (r.phutConLaiDeThanhToan != null) {
            const total = 15; // giả định 15 phút
            const remain = r.phutConLaiDeThanhToan;
            const percent = Math.min(100, Math.max(0, (remain / total) * 100));
            return (
              <div className="space-y-1">
                <Progress
                  percent={Math.round(percent)}
                  size="small"
                  status={percent < 15 ? "exception" : "active"}
                  showInfo={false}
                  strokeColor={percent < 30 ? "#f97316" : "#16a34a"}
                />
                <div className="text-[11px] text-slate-600">
                  Còn {remain} phút
                </div>
              </div>
            );
          }
          return <span className="text-xs text-slate-500">—</span>;
        },
      },
      {
        key: "status",
        title: "Trạng thái",
        width: 150,
        dataIndex: "trangThai",
        render: (v: LichSuDatPhongDTO["trangThai"]) => {
          const info = STATUS_INFO[v];
          return <Tag color={info.color}>{info.label}</Tag>;
        },
      },
      {
        key: "tongTien",
        title: "Tổng tiền",
        dataIndex: "tongTien",
        width: 130,
        align: "right" as const,
        render: (v: number) => (
          <span className="font-semibold text-emerald-600">
            {v.toLocaleString("vi-VN")} đ
          </span>
        ),
      },
      {
        key: "ngayTao",
        title: "Ngày tạo",
        dataIndex: "ngayTao",
        width: 160,
        render: (v: string) => dayjs(v).format("DD/MM/YYYY HH:mm"),
      },
    ],
    [],
  );

  // reuse columns variable (WITHOUT actions)
  const baseColumns = useMemo(() => columns, [columns]);

  // Actions per row
  const actionRenderer = (r: LichSuDatPhongDTO) => {
    if (r.coTheHuy)
      return (
        <Button size="small" danger onClick={() => cancel(r.maThuePhong)}>
          Hủy
        </Button>
      );
    if (r.coTheXacNhanThanhToan && r.maHoaDon)
      return (
        <Button
          size="small"
          type="primary"
          loading={confirmingPayment && actingId === r.maThuePhong}
          onClick={() => handlePayExisting(r)}
          className="bg-gradient-to-r from-indigo-500 to-indigo-600 hover:from-indigo-600 hover:to-indigo-700 border-none"
        >
          Thanh toán
        </Button>
      );
    if (r.coTheThanhToanLai)
      return (
        <Button
          size="small"
          loading={rePayLoading && actingId === r.maThuePhong}
          onClick={() => handleRePay(r)}
          className="bg-gradient-to-r from-amber-500 to-amber-600 hover:from-amber-600 hover:to-amber-700 border-none text-white"
        >
          Thanh toán lại
        </Button>
      );
    return null;
  };

  const tableAll = (
    <GenericQLTable<LichSuDatPhongDTO>
      data={history}
      loading={historyLoading}
      columns={baseColumns as any}
      rowKey="maThuePhong"
      tableName="lịch sử"
      showUpdateAction={false}
      showDeleteAction={false}
      extraRowActions={actionRenderer}
    />
  );

  const tableUnpaid = (
    <GenericQLTable<LichSuDatPhongDTO>
      data={unpaid.filter(
        (x: LichSuDatPhongDTO) =>
          x.trangThai === "ChuaThanhToan" || x.trangThai === "HetHanThanhToan",
      )}
      loading={unpaidLoading}
      columns={baseColumns as any}
      rowKey="maThuePhong"
      tableName="chưa thanh toán"
      showUpdateAction={false}
      showDeleteAction={false}
      extraRowActions={actionRenderer}
    />
  );

  // Handlers (add near top in component)
  const [actingId, setActingId] = useState<string | null>(null);

  // Thanh toán lại
  const handleRePay = async (r: LichSuDatPhongDTO) => {
    setActingId(r.maThuePhong);
    const rs = await rePay(r.maThuePhong);
    if (!rs.success) message.error(rs.error || "Không tạo được URL thanh toán");
    setActingId(null);
  };
  // Thanh toán hóa đơn còn hạn (dùng confirmPayment thunk)
  const handlePayExisting = async (r: LichSuDatPhongDTO) => {
    if (!r.maHoaDon) return;
    setActingId(r.maThuePhong);
    const rs = await confirm({
      maHoaDon: r.maHoaDon,
      maThuePhong: r.maThuePhong,
    });
    if (!rs.success) message.error(rs.error || "Không tạo được URL thanh toán");
    setActingId(null);
  };

  return (
    <CustomerLayout>
      <div className="min-h-screen bg-gradient-to-br from-indigo-600 to-indigo-900">
        <div className="max-w-7xl mx-auto px-6 py-16">
          <div className="bg-white/10 backdrop-blur rounded-3xl p-8 shadow-2xl space-y-8 mt-4">
            <div className="flex items-center justify-between flex-wrap gap-4">
              <h1 className="text-3xl font-bold text-white m-0 flex items-center gap-3">
                <div className="w-1 h-8 bg-gradient-to-b from-yellow-400 to-orange-500 rounded-full" />
                Lịch sử đặt phòng
              </h1>
              <Button
                onClick={() => loadHistory?.()}
                size="large"
                className="bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 border-none text-white font-medium shadow-lg"
              >
                🔄 Làm mới
              </Button>
            </div>
            <Tabs
              defaultActiveKey="all"
              className="px-4 pt-4"
              items={[
                { key: "all", label: "Tất cả", children: tableAll },
                {
                  key: "unpaid",
                  label: "Chưa thanh toán",
                  children: tableUnpaid,
                },
              ]}
            />
          </div>
        </div>
      </div>
    </CustomerLayout>
  );
};
