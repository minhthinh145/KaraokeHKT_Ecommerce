import React, { useMemo, useState } from "react";
import { Button, Input, Modal, Form, Tag, Space } from "antd";
import {
  SearchOutlined,
  CheckOutlined,
  CloseOutlined,
} from "@ant-design/icons";
import dayjs from "dayjs";
import { GenericQLTable } from "../../uiForAll/GenericQLTable";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu";
import type { YeuCauChuyenCaDTO } from "../../../../api";
import "dayjs/locale/vi";
dayjs.locale("vi");
interface XemYeuCauTableProps {
  weekStart: string;
}

export const XemYeuCauTable: React.FC<XemYeuCauTableProps> = ({
  weekStart,
}) => {
  const { pheDuyetLists, pheDuyetLoading, pheDuyetApproving, pheDuyetActions } =
    useQLNhanSu();

  const [search, setSearch] = useState("");
  const [modal, setModal] = useState<{
    open: boolean;
    mode: "APPROVE" | "REJECT";
    record?: YeuCauChuyenCaDTO;
  }>({ open: false, mode: "APPROVE" });
  const [form] = Form.useForm();

  const weekDates = useMemo(
    () =>
      new Set(
        Array.from({ length: 7 }, (_, i) =>
          dayjs(weekStart).add(i, "day").format("YYYY-MM-DD")
        )
      ),
    [weekStart]
  );

  const dataWeek = useMemo(
    () =>
      (pheDuyetLists.all || []).filter((x: any) => {
        const g = dayjs(x.ngayLamViecGoc).format("YYYY-MM-DD");
        const m = dayjs(x.ngayLamViecMoi).format("YYYY-MM-DD");
        return weekDates.has(g) || weekDates.has(m);
      }),
    [pheDuyetLists.all, weekDates]
  );

  const filtered = useMemo(
    () =>
      dataWeek.filter((r: any) => {
        if (!search) return true;
        const k = search.toLowerCase();
        return (
          (r.tenNhanVien || "").toString().toLowerCase().includes(k) ||
          (r.tenCaGoc || "").toLowerCase().includes(k) ||
          (r.tenCaMoi || "").toLowerCase().includes(k)
        );
      }),
    [dataWeek, search]
  );

  const openModal = (r: YeuCauChuyenCaDTO, mode: "APPROVE" | "REJECT") => {
    setModal({ open: true, record: r, mode });
    form.resetFields();
  };
  const closeModal = () =>
    setModal((s) => ({ ...s, open: false, record: undefined }));

  const handleSubmit = async () => {
    if (!modal.record) return;
    const ghiChu = form.getFieldValue("ghiChu") || "";
    await pheDuyetActions.approve({
      maYeuCau: (modal.record as any).maYeuCau,
      ghiChu,
      ketQua: modal.mode === "APPROVE",
    });
    closeModal();
  };

  const columns = [
    {
      key: "nhanVien",
      title: "Nhân viên",
      align: "center" as const,
      render: (_: any, r: YeuCauChuyenCaDTO) => (
        <div style={{ fontWeight: 600 }}>{(r as any).tenNhanVien || "-"}</div>
      ),
    },
    {
      key: "lichGoc",
      title: "Lịch gốc",
      align: "center" as const,
      render: (_: any, r: YeuCauChuyenCaDTO) => (
        <div>
          <div style={{ fontWeight: 600 }}>{(r as any).tenCaGoc}</div>
          <div style={{ fontSize: 12, color: "#888" }}>
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
          <div style={{ fontWeight: 600 }}>{(r as any).tenCaMoi}</div>
          <div style={{ fontSize: 12, color: "#888" }}>
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
      key: "trangThai",
      title: "Trạng thái",
      align: "center" as const,
      render: (_: any, r: YeuCauChuyenCaDTO) =>
        (r as any).daPheDuyet ? (
          <Tag color="green">Đã duyệt</Tag>
        ) : (r as any).daTuChoi ? (
          <Tag color="red">Đã từ chối</Tag>
        ) : (
          <Tag color="orange">Chờ duyệt</Tag>
        ),
    },
    {
      key: "ketQuaPheDuyet",
      title: "Kết quả",
      align: "center" as const,
      render: (_: any, r: YeuCauChuyenCaDTO) => {
        const val = (r as any).ketQuaPheDuyet ?? (r as any).ketQua ?? null;
        if (val === true) return <Tag color="green">Đồng ý</Tag>;
        if (val === false) return <Tag color="red">Từ chối</Tag>;
        return <span className="text-gray-500">Chưa xử lý</span>;
      },
    },
    {
      key: "ghiChuNhanVien",
      title: "Ghi chú NV",
      align: "center" as const,
      render: (_: any, r: YeuCauChuyenCaDTO) =>
        (r as any).lyDoChuyenCa ? (
          <span>{(r as any).lyDoChuyenCa}</span>
        ) : (
          <span className="text-gray-400 italic">Không có ghi chú</span>
        ),
    },
    {
      key: "ghiChu",
      title: "Ghi chú phê duyệt",
      align: "center" as const,
      render: (_: any, r: YeuCauChuyenCaDTO) =>
        (r as any).daPheDuyet ? (
          (r as any).ghiChuPheDuyet ? (
            <span>{(r as any).ghiChuPheDuyet}</span>
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
      width: 180,
      render: (_: any, r: YeuCauChuyenCaDTO) =>
        (r as any).daPheDuyet || (r as any).daTuChoi ? (
          <span className="text-xs text-gray-400">Đã duyệt</span>
        ) : (
          <Space>
            <Button
              size="small"
              type="primary"
              icon={<CheckOutlined />}
              loading={pheDuyetApproving}
              onClick={() => openModal(r, "APPROVE")}
            >
              Duyệt
            </Button>
            <Button
              size="small"
              danger
              icon={<CloseOutlined />}
              loading={pheDuyetApproving}
              onClick={() => openModal(r, "REJECT")}
            >
              Từ chối
            </Button>
          </Space>
        ),
    },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h3 className="text-base font-semibold m-0">
          Yêu cầu chuyển ca (tuần)
        </h3>
        <Input
          allowClear
          placeholder="Tìm kiếm..."
          prefix={<SearchOutlined />}
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          style={{ maxWidth: 280 }}
        />
      </div>
      <GenericQLTable<YeuCauChuyenCaDTO>
        data={filtered}
        loading={pheDuyetLoading}
        columns={columns}
        rowKey="maYeuCau"
        tableName="yêu cầu"
        emptyMessage="Không có yêu cầu trong tuần"
      />

      <Modal
        open={modal.open}
        title={
          modal.mode === "APPROVE" ? "Phê duyệt yêu cầu" : "Từ chối yêu cầu"
        }
        onCancel={closeModal}
        onOk={handleSubmit}
        okText={modal.mode === "APPROVE" ? "Duyệt" : "Từ chối"}
        okButtonProps={{
          type: "primary",
          danger: modal.mode === "REJECT",
          loading: pheDuyetApproving,
        }}
        cancelText="Hủy"
      >
        {modal.record && (
          <div className="space-y-3">
            <div className="text-sm">
              Ca gốc: <b>{(modal.record as any).tenCaGoc}</b> (
              {dayjs((modal.record as any).ngayLamViecGoc).format("DD/MM/YYYY")}
              )
            </div>
            <div className="text-sm">
              Ca mới: <b>{(modal.record as any).tenCaMoi}</b> (
              {dayjs((modal.record as any).ngayLamViecMoi).format("DD/MM/YYYY")}
              )
            </div>
            <Form form={form} layout="vertical">
              <Form.Item name="ghiChu" label="Ghi chú">
                <Input.TextArea
                  placeholder="Nhập ghi chú (tuỳ chọn)"
                  autoSize={{ minRows: 2, maxRows: 4 }}
                />
              </Form.Item>
            </Form>
          </div>
        )}
      </Modal>
    </div>
  );
};
