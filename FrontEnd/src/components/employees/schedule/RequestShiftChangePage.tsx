import React from "react";
import { Form, DatePicker, Select, Input, Button, Table, Tag } from "antd";

const { TextArea } = Input;

const mockRequests = [
  {
    id: 1,
    ngayCu: "2024-01-15",
    caCu: "Ca 1",
    ngayMoi: "2024-01-20",
    caMoi: "Ca 2",
    lyDo: "Có việc gia đình",
    trangThai: "ChoDuyet",
  },
];

const columns = [
  { title: "Ngày cũ", dataIndex: "ngayCu", width: 100 },
  { title: "Ca cũ", dataIndex: "caCu", width: 80 },
  { title: "Ngày mới", dataIndex: "ngayMoi", width: 100 },
  { title: "Ca mới", dataIndex: "caMoi", width: 80 },
  { title: "Lý do", dataIndex: "lyDo", ellipsis: true },
  {
    title: "Trạng thái",
    dataIndex: "trangThai",
    width: 120,
    render: (status: string) => (
      <Tag
        color={
          status === "ChapNhan" ? "green" : status === "TuChoi" ? "red" : "gold"
        }
      >
        {status === "ChapNhan"
          ? "Chấp nhận"
          : status === "TuChoi"
          ? "Từ chối"
          : "Chờ duyệt"}
      </Tag>
    ),
  },
];

export const RequestShiftChangePage: React.FC = () => {
  const [form] = Form.useForm();

  const onFinish = (values: any) => {
    form.resetFields();
  };

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-xl font-semibold text-gray-800 mb-2">
          Yêu cầu đổi ca làm việc
        </h2>
        <p className="text-gray-600">Gửi yêu cầu đổi ca làm việc cho quản lý</p>
      </div>

      <Form
        form={form}
        layout="vertical"
        onFinish={onFinish}
        className="bg-gray-50 p-4 rounded-lg"
      >
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <Form.Item
            name="ngayCu"
            label="Ngày hiện tại"
            rules={[{ required: true, message: "Chọn ngày hiện tại" }]}
          >
            <DatePicker className="w-full" format="DD/MM/YYYY" />
          </Form.Item>

          <Form.Item
            name="caCu"
            label="Ca hiện tại"
            rules={[{ required: true, message: "Chọn ca hiện tại" }]}
          >
            <Select
              placeholder="Chọn ca"
              options={[
                { value: "Ca1", label: "Ca 1" },
                { value: "Ca2", label: "Ca 2" },
                { value: "Ca3", label: "Ca 3" },
              ]}
            />
          </Form.Item>

          <Form.Item
            name="ngayMoi"
            label="Ngày muốn đổi"
            rules={[{ required: true, message: "Chọn ngày muốn đổi" }]}
          >
            <DatePicker className="w-full" format="DD/MM/YYYY" />
          </Form.Item>

          <Form.Item
            name="caMoi"
            label="Ca muốn đổi"
            rules={[{ required: true, message: "Chọn ca muốn đổi" }]}
          >
            <Select
              placeholder="Chọn ca"
              options={[
                { value: "Ca1", label: "Ca 1" },
                { value: "Ca2", label: "Ca 2" },
                { value: "Ca3", label: "Ca 3" },
              ]}
            />
          </Form.Item>
        </div>

        <Form.Item
          name="lyDo"
          label="Lý do đổi ca"
          rules={[{ required: true, message: "Nhập lý do đổi ca" }]}
        >
          <TextArea
            rows={3}
            placeholder="Nhập lý do muốn đổi ca..."
            maxLength={500}
            showCount
          />
        </Form.Item>

        <Button type="primary" htmlType="submit">
          Gửi yêu cầu
        </Button>
      </Form>

      <div>
        <h3 className="text-lg font-medium text-gray-800 mb-4">
          Lịch sử yêu cầu
        </h3>
        <Table
          size="small"
          columns={columns}
          dataSource={mockRequests}
          rowKey="id"
          pagination={{ pageSize: 10 }}
        />
      </div>
    </div>
  );
};
