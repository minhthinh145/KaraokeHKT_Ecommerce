import React, { useEffect } from "react";
import { Modal, Form, Input, InputNumber, Typography, Alert } from "antd";
import { EditOutlined, InfoCircleOutlined } from "@ant-design/icons";
import type {
  LoaiPhongDTO,
  UpdateLoaiPhongDTO,
} from "../../../../../redux/admin/QLPhong/types";

const { Title, Text } = Typography;

interface Props {
  open: boolean;
  onClose: () => void;
  row: LoaiPhongDTO | null;
  onSubmit: (payload: UpdateLoaiPhongDTO) => Promise<{ success: boolean }>;
}

export const EditLoaiPhongModal: React.FC<Props> = ({
  open,
  onClose,
  row,
  onSubmit,
}) => {
  const [form] = Form.useForm<UpdateLoaiPhongDTO>();

  useEffect(() => {
    if (row)
      form.setFieldsValue({
        maLoaiPhong: row.maLoaiPhong,
        tenLoaiPhong: row.tenLoaiPhong,
        sucChua: row.sucChua,
      });
  }, [row, form]);

  return (
    <Modal
      open={open}
      onCancel={onClose}
      onOk={() => form.submit()}
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-indigo-500 rounded-full mb-3">
            <EditOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Cập nhật loại phòng
          </Title>
        </div>
      }
      width={600}
      destroyOnClose
    >
      <Form form={form} layout="vertical" onFinish={(v) => onSubmit(v)}>
        <Alert
          type="info"
          showIcon
          className="mb-4 rounded-lg"
          message={
            <div>
              <Text strong>Đang chỉnh sửa</Text>
              <div className="text-slate-600 mt-1 text-sm">
                Mã loại phòng: <Text strong>{row?.maLoaiPhong}</Text>
              </div>
            </div>
          }
          icon={<InfoCircleOutlined />}
        />
        <Form.Item name="maLoaiPhong" hidden>
          <InputNumber />
        </Form.Item>
        <Form.Item
          name="tenLoaiPhong"
          label="Tên loại phòng"
          rules={[{ required: true }]}
        >
          <Input />
        </Form.Item>
        <Form.Item name="sucChua" label="Sức chứa" rules={[{ required: true }]}>
          <InputNumber min={1} className="w-full" addonAfter="người" />
        </Form.Item>
      </Form>
    </Modal>
  );
};
