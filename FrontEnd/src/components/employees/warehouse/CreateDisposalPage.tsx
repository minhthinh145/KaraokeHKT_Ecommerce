import React from "react";
import { Form, Input, InputNumber, Button, Select } from "antd";
import { PlusOutlined } from "@ant-design/icons";

const { TextArea } = Input;

export const CreateDisposalPage: React.FC = () => {
  const [form] = Form.useForm();

  const onFinish = (values: any) => {};

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-xl font-semibold text-gray-800 mb-2">
          Tạo phiếu hủy hàng
        </h2>
        <p className="text-gray-600">
          Tạo phiếu hủy cho vật tư hỏng hoặc hết hạn
        </p>
      </div>

      <Form form={form} layout="vertical" onFinish={onFinish}>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <Form.Item
            name="soPhieu"
            label="Số phiếu hủy"
            rules={[{ required: true, message: "Nhập số phiếu" }]}
          >
            <Input placeholder="Nhập số phiếu hủy..." />
          </Form.Item>

          <Form.Item
            name="lyDoHuy"
            label="Lý do hủy"
            rules={[{ required: true, message: "Chọn lý do hủy" }]}
          >
            <Select
              placeholder="Chọn lý do"
              options={[
                { value: "HetHan", label: "Hết hạn sử dụng" },
                { value: "Hong", label: "Hỏng hóc" },
                { value: "KhongDamBao", label: "Không đảm bảo chất lượng" },
                { value: "Khac", label: "Lý do khác" },
              ]}
            />
          </Form.Item>
        </div>

        <div className="bg-gray-50 p-4 rounded-lg">
          <h3 className="font-medium mb-4">Chi tiết vật tư hủy</h3>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
            <Input placeholder="Tên vật tư" />
            <InputNumber
              placeholder="Số lượng hủy"
              className="w-full"
              min={1}
            />
            <Button type="primary" icon={<PlusOutlined />}>
              Thêm
            </Button>
          </div>
        </div>

        <Form.Item name="ghiChu" label="Ghi chú">
          <TextArea
            rows={3}
            placeholder="Ghi chú thêm..."
            maxLength={500}
            showCount
          />
        </Form.Item>

        <Button type="primary" htmlType="submit" size="large">
          Tạo phiếu hủy
        </Button>
      </Form>
    </div>
  );
};
