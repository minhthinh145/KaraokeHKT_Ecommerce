import React from "react";
import { Modal, Form, Select, DatePicker, Input } from "antd";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (p: {
    maLichLamViecGoc: number;
    ngayLamViecMoi: string;
    maCaMoi: number;
    lyDoChuyenCa?: string;
  }) => void;
  caOptions: { value: number; label: string }[];
  maLichLamViecGoc?: number;
}

export const ModalCreateShiftChange: React.FC<Props> = ({
  open,
  onClose,
  onSubmit,
  caOptions,
  maLichLamViecGoc,
}) => {
  const [form] = Form.useForm();
  return (
    <Modal
      open={open}
      onCancel={onClose}
      onOk={() => form.submit()}
      title={
        <div className="text-center py-2">
          <div className="text-lg font-bold bg-gradient-to-r from-indigo-500 to-purple-500 text-transparent bg-clip-text">
            Tạo yêu cầu chuyển ca
          </div>
        </div>
      }
      destroyOnClose
    >
      <Form
        layout="vertical"
        form={form}
        initialValues={{ maLichLamViecGoc }}
        onFinish={(v) => {
          onSubmit({
            maLichLamViecGoc: v.maLichLamViecGoc,
            ngayLamViecMoi: v.ngayLamViecMoi.format("YYYY-MM-DD"),
            maCaMoi: v.maCaMoi,
            lyDoChuyenCa: v.lyDoChuyenCa?.trim(),
          });
          onClose();
          form.resetFields();
        }}
      >
        <Form.Item name="maLichLamViecGoc" label="Lịch gốc">
          <input
            disabled
            className="w-full border rounded px-2 py-1 text-sm bg-gray-50"
          />
        </Form.Item>
        <Form.Item
          name="ngayLamViecMoi"
          label="Ngày mới"
          rules={[{ required: true }]}
        >
          <DatePicker className="w-full" format="DD/MM/YYYY" />
        </Form.Item>
        <Form.Item name="maCaMoi" label="Ca mới" rules={[{ required: true }]}>
          <Select options={caOptions} placeholder="Chọn ca" />
        </Form.Item>
        <Form.Item name="lyDoChuyenCa" label="Lý do">
          <Input.TextArea rows={3} maxLength={500} showCount />
        </Form.Item>
        <div className="text-xs text-slate-500">
          Lưu ý: Không tạo trùng yêu cầu đang chờ duyệt cho cùng lịch.
        </div>
      </Form>
    </Modal>
  );
};
