import React, { useEffect } from "react";
import { Modal, Form, InputNumber } from "antd";
import type { VatLieuDetailDTO } from "../../../../../api/index";

interface Props {
  open: boolean;
  row?: VatLieuDetailDTO | null;
  onClose: () => void;
  onSubmit: (
    maVatLieu: number,
    soLuongMoi: number
  ) => Promise<{ success: boolean } | void>;
}

export const UpdateSoLuongModal: React.FC<Props> = ({
  open,
  row,
  onClose,
  onSubmit,
}) => {
  const [form] = Form.useForm();
  useEffect(() => {
    if (row) form.setFieldsValue({ soLuongMoi: row.soLuongTonKho });
  }, [row]);

  return (
    <Modal
      open={open}
      onCancel={onClose}
      title="Cập nhật số lượng"
      onOk={() => form.submit()}
    >
      <Form
        form={form}
        layout="vertical"
        onFinish={(v) =>
          row && onSubmit(row.maVatLieu, Number(v.soLuongMoi || 0))
        }
      >
        <Form.Item
          label="Số lượng mới"
          name="soLuongMoi"
          rules={[{ required: true }]}
        >
          <InputNumber min={0} className="w-full" />
        </Form.Item>
      </Form>
    </Modal>
  );
};
