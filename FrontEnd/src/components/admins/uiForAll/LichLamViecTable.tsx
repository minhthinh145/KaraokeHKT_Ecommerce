import React, { useMemo, useState, useRef } from "react";
import dayjs from "dayjs";
import { Button, Card, Table, Tag, Modal } from "antd";
import { PlusOutlined, PrinterOutlined, MailOutlined } from "@ant-design/icons";
import { EmployeeCard } from "./EmployeeCard";
import type { LichLamViecDTO } from "../../../api";
import { ConfirmDialog } from "../uiForAll/ConfirmDialog";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";
import { LichLamViecTablePrint } from "./LichLamViecTablePrint";
import { LichLamViecChild } from "./LichLamViecChild/LichLamViecChild";

export interface CaBrief {
  maCa: number;
  tenCaLamViec: string;
  gioBatDauCa: string;
  gioKetThucCa: string;
}

interface LichLamViecTableProps {
  caList: CaBrief[];
  weekStart: string;
  data: LichLamViecDTO[];
  getNhanVienName?: (item: LichLamViecDTO) => string;
  getNhanVienInfo?: (item: LichLamViecDTO) => string | undefined;
  canAdd?: boolean;
  canEdit?: boolean;
  canDelete?: boolean;
  onAdd?: (payload: { ngayLamViec: string; maCa: number }) => void;
  onEdit?: (item: LichLamViecDTO) => void;
  onDelete?: (item: LichLamViecDTO) => void;
  renderCard?: (item: LichLamViecDTO) => React.ReactNode;
  className?: string;
  onPrint?: () => void;
  loading?: boolean;
  onSendNoti?: (payload: { start: string; end: string }) => void;
  sendingNoti?: boolean;
  // NEW PROPS
  showSendNoti?: boolean; // mặc định true (nhân viên truyền false)
  enableShiftRequestDrag?: boolean;
  draggingLichId?: number | null;
  onShiftDragStart?: (maLichLamViec: number) => void;
  onShiftDragEnd?: () => void;
  onShiftDropCreate?: (p: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }) => void;
  showShiftRequestHint?: boolean;
  pendingShiftChanges?: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
  }[];
}

export const LichLamViecTable: React.FC<LichLamViecTableProps> = ({
  caList,
  weekStart,
  data,
  getNhanVienName = (item) => item.tenNhanVien || item.maNhanVien,
  getNhanVienInfo = (item) => item.loaiNhanVien,
  canAdd = true,
  canEdit = true,
  canDelete = true,
  onAdd,
  onEdit,
  onDelete,
  renderCard, // (không còn dùng trong child mới – nếu cần có thể bổ sung sau)
  className = "",
  onPrint,
  loading = false,
  onSendNoti,
  sendingNoti = false,
  showSendNoti = true,
  enableShiftRequestDrag = false,
  draggingLichId = null,
  onShiftDragStart,
  onShiftDragEnd,
  onShiftDropCreate,
  showShiftRequestHint = true,
  pendingShiftChanges = [],
}) => {
  const days = useMemo(
    () => Array.from({ length: 7 }, (_, i) => dayjs(weekStart).add(i, "day")),
    [weekStart]
  );
  const today = dayjs().format("YYYY-MM-DD");

  const tableMap = useMemo(() => {
    const m = new Map<string, LichLamViecDTO[]>();
    for (const item of data) {
      const day = dayjs(item.ngayLamViec).format("YYYY-MM-DD");
      const key = `${item.maCa}_${day}`;
      if (!m.has(key)) m.set(key, []);
      m.get(key)!.push(item);
    }
    for (const [k, arr] of m) {
      arr.sort((a, b) =>
        (getNhanVienName(a) || "").localeCompare(getNhanVienName(b) || "", "vi")
      );
      m.set(k, arr);
    }
    return m;
  }, [data, getNhanVienName]);

  // Confirm delete logic retained
  const [confirmState, setConfirmState] = useState<{
    open: boolean;
    item?: LichLamViecDTO;
  }>({ open: false });
  const handleDelete = (item: LichLamViecDTO) =>
    setConfirmState({ open: true, item });
  const handleConfirm = async () => {
    if (confirmState.item) await onDelete?.(confirmState.item);
    setConfirmState({ open: false });
  };
  const handleCancel = () => setConfirmState({ open: false });

  // PRINT logic kept (unchanged)
  const containerRef = React.useRef<HTMLDivElement>(null);
  const printAreaRef = useRef<HTMLDivElement>(null);
  const [printOpen, setPrintOpen] = useState(false);
  const handleOpenPrintPreview = () => setPrintOpen(true);
  const handleClosePrintPreview = () => setPrintOpen(false);

  const handleBrowserPrint = () => {
    const style = document.createElement("style");
    style.media = "print";
    style.innerHTML = `
      @page { size: A4 landscape; margin: 10mm; }
      body * { visibility: hidden !important; }
      #print-area, #print-area * { visibility: visible !important; }
      #print-area { position: absolute; inset: 0; width: 100%; }
    `;
    document.head.appendChild(style);
    window.print();
    setTimeout(() => document.head.removeChild(style), 0);
  };

  const handleExportPdf = async () => {
    const el = printAreaRef.current;
    if (!el) return;
    const canvas = await html2canvas(el, {
      scale: Math.max(2, Math.min(3, window.devicePixelRatio || 2)),
      backgroundColor: "#fff",
      useCORS: true,
    });
    const imgData = canvas.toDataURL("image/png");
    const pdf = new jsPDF({
      orientation: "landscape",
      unit: "pt",
      format: "a4",
    });
    const pageWidth = pdf.internal.pageSize.getWidth();
    const pageHeight = pdf.internal.pageSize.getHeight();
    const imgWidth = pageWidth;
    const imgHeight = (canvas.height * pageWidth) / canvas.width;
    pdf.addImage(
      imgData,
      "PNG",
      0,
      0,
      imgWidth,
      Math.min(imgHeight, pageHeight)
    );
    pdf.save("lich-lam-viec.pdf");
  };

  // Thêm các biến này vào đầu component:
  const pendingSourceIds = useMemo(
    () => new Set(pendingShiftChanges.map((p) => p.maLichLamViecGoc)),
    [pendingShiftChanges]
  );
  const pendingTargetKeySet = useMemo(
    () =>
      new Set(
        pendingShiftChanges.map(
          (p) => `${p.maCaMoi}_${dayjs(p.ngayLamViecMoi).format("YYYY-MM-DD")}`
        )
      ),
    [pendingShiftChanges]
  );
  const sourceIdToReq = useMemo(
    () => new Map(pendingShiftChanges.map((p) => [p.maLichLamViecGoc, p])),
    [pendingShiftChanges]
  );

  return (
    <div className={`w-full ${className}`}>
      <Card className="shadow-sm">
        <div className="flex justify-end mb-2 gap-2">
          <Button
            icon={<PrinterOutlined />}
            type="primary"
            onClick={() => setPrintOpen(true)}
            size="middle"
            loading={loading}
          >
            In / PDF
          </Button>
          {showSendNoti && (
            <Button
              icon={<MailOutlined />}
              type="primary"
              onClick={() => {
                const start = dayjs(weekStart).format("YYYY-MM-DD");
                const end = dayjs(weekStart).add(6, "day").format("YYYY-MM-DD");
                onSendNoti?.({ start, end });
              }}
              loading={sendingNoti}
            >
              Gửi thông báo
            </Button>
          )}
        </div>

        {loading && (
          <div className="text-center text-sm text-gray-500 py-4">
            Đang tải dữ liệu...
          </div>
        )}
        {!loading && (
          <LichLamViecChild
            caList={caList}
            days={days}
            today={today}
            tableMap={tableMap}
            getNhanVienName={getNhanVienName}
            getNhanVienInfo={getNhanVienInfo}
            canAdd={canAdd}
            canEdit={canEdit}
            canDelete={canDelete}
            onAdd={onAdd}
            onEdit={onEdit}
            onDelete={handleDelete}
            enableShiftRequestDrag={enableShiftRequestDrag}
            draggingLichId={draggingLichId}
            onShiftDragStart={onShiftDragStart}
            onShiftDragEnd={onShiftDragEnd}
            onShiftDropCreate={onShiftDropCreate}
            showShiftRequestHint={showShiftRequestHint}
            pendingShiftChanges={pendingShiftChanges}
            schedule={data}
          />
        )}
      </Card>

      {/* Print modal (unchanged original content) */}
      <Modal
        open={printOpen}
        onCancel={() => setPrintOpen(false)}
        width="90vw"
        title="Xem trước lịch làm việc"
        footer={[
          <Button key="cancel" onClick={() => setPrintOpen(false)}>
            Đóng
          </Button>,
          <Button key="print" type="primary" onClick={handleBrowserPrint}>
            In
          </Button>,
          <Button key="pdf" onClick={handleExportPdf}>
            Tải PDF
          </Button>,
        ]}
        bodyStyle={{ background: "#fff", padding: 16 }}
      >
        <div
          style={{
            width: "100%",
            overflowX: "auto",
            display: "flex",
            justifyContent: "center",
          }}
        >
          <div
            id="print-area"
            ref={printAreaRef}
            style={{ width: 1400, maxWidth: "100%" }}
          >
            <LichLamViecTablePrint
              caList={caList}
              weekStart={weekStart}
              data={data}
              getNhanVienName={getNhanVienName}
              getNhanVienInfo={getNhanVienInfo}
            />
          </div>
        </div>
      </Modal>

      <ConfirmDialog
        isOpen={confirmState.open}
        title="Xác nhận xóa lịch làm việc"
        message="Bạn có chắc chắn muốn xóa lịch làm việc này không?"
        confirmText="Xóa"
        cancelText="Hủy"
        confirmButtonVariant="danger"
        onConfirm={handleConfirm}
        onCancel={handleCancel}
      />
    </div>
  );
};

/*
@keyframes dash {
  0% { stroke-dashoffset: 12; opacity: .4; }
  50% { opacity: 1; }
  100% { stroke-dashoffset: 0; opacity: .4; }
}
*/
