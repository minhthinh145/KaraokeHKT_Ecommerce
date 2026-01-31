import React from "react";
import { Modal, Form, Input, InputNumber, Typography, Alert } from "antd";
import { PlusOutlined, InfoCircleOutlined } from "@ant-design/icons";
import type { AddLoaiPhongDTO } from "../../../../../redux/admin/QLPhong/types";

const { Title, Text } = Typography;

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (payload: AddLoaiPhongDTO) => Promise<{ success: boolean }>;
}

export const CreateLoaiPhongModal: React.FC<Props> = ({
  open,
  onClose,
  onSubmit,
}) => {
  const [form] = Form.useForm<AddLoaiPhongDTO>();

  return (
    <Modal
      open={open}
      onCancel={onClose}
      onOk={() => form.submit()}
      title={
        <div className="text-center py-2">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-emerald-500 to-green-600 rounded-full mb-3">
            <PlusOutlined className="text-2xl text-white" />
          </div>
          <Title level={3} className="!mb-1 !text-slate-800">
            Thêm loại phòng
          </Title>
        </div>
      }
      destroyOnClose
    >
      <Form form={form} layout="vertical" onFinish={(v) => onSubmit(v)}>
        <Alert
          type="info"
          showIcon
          className="mb-4 rounded-lg"
          message={
            <div>
              <Text strong>Hướng dẫn</Text>
              <div className="text-slate-600 mt-1 text-sm">
                Nhập tên loại phòng và sức chứa (đơn vị: người).
              </div>
            </div>
          }
          icon={<InfoCircleOutlined />}
        />
        <Form.Item
          name="tenLoaiPhong"
          label="Tên loại phòng"
          rules={[{ required: true, message: "Nhập tên loại phòng" }]}
        >
          <Input placeholder="VD: VIP, Standard..." />
        </Form.Item>
        <Form.Item
          name="sucChua"
          label="Sức chứa"
          rules={[{ required: true, message: "Nhập sức chứa" }]}
        >
          <InputNumber min={1} className="w-full" addonAfter="người" />
        </Form.Item>
      </Form>
    </Modal>
  );
};
