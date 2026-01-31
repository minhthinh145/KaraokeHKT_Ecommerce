import React from "react";
import { Form, Input, InputNumber, Button, Table, Tag } from "antd";
import { PlusOutlined, DeleteOutlined } from "@ant-design/icons";

export const CreateImportPage: React.FC = () => {
  const [form] = Form.useForm();

  const onFinish = () => {};

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-xl font-semibold text-gray-800 mb-2">
          Tạo phiếu nhập hàng
        </h2>
        <p className="text-gray-600">Tạo phiếu nhập hàng mới vào kho</p>
      </div>

      <Form form={form} layout="vertical" onFinish={onFinish}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
          <Form.Item
            name="soPhieu"
            label="Số phiếu"
            rules={[{ required: true, message: "Nhập số phiếu" }]}
          >
            <Input placeholder="Nhập số phiếu..." />
          </Form.Item>

          <Form.Item
            name="nhaCungCap"
            label="Nhà cung cấp"
            rules={[{ required: true, message: "Nhập nhà cung cấp" }]}
          >
            <Input placeholder="Nhập tên nhà cung cấp..." />
          </Form.Item>
        </div>

        <div className="bg-gray-50 p-4 rounded-lg">
          <h3 className="font-medium mb-4">Chi tiết vật tư nhập</h3>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4">
            <Input placeholder="Tên vật tư" />
            <InputNumber placeholder="Số lượng" className="w-full" min={1} />
            <InputNumber placeholder="Đơn giá" className="w-full" min={0} />
            <Button type="primary" icon={<PlusOutlined />}>
              Thêm
            </Button>
          </div>
        </div>

        <Button type="primary" htmlType="submit" size="large">
          Tạo phiếu nhập
        </Button>
      </Form>
    </div>
  );
};
