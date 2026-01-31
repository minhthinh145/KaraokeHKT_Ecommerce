import React from "react";
import { Table, Tag, Input } from "antd";
import { SearchOutlined } from "@ant-design/icons";

const mockData = [
  {
    id: 1,
    tenVatLieu: "Coca Cola",
    donViTinh: "Chai",
    soLuongTon: 150,
    trangThai: "ConHang",
  },
  {
    id: 2,
    tenVatLieu: "Sting",
    donViTinh: "Lon",
    soLuongTon: 5,
    trangThai: "SapHet",
  },
];

const columns = [
  { title: "Tên vật tư", dataIndex: "tenVatLieu" },
  { title: "Đơn vị", dataIndex: "donViTinh", width: 100 },
  { title: "Số lượng tồn", dataIndex: "soLuongTon", width: 120 },
  {
    title: "Trạng thái",
    dataIndex: "trangThai",
    width: 120,
    render: (status: string) => (
      <Tag color={status === "ConHang" ? "green" : "orange"}>
        {status === "ConHang" ? "Còn hàng" : "Sắp hết"}
      </Tag>
    ),
  },
];

export const ViewWarehousePage: React.FC = () => {
  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-xl font-semibold text-gray-800">Kho hàng</h2>
          <p className="text-gray-600">Xem tình trạng vật tư trong kho</p>
        </div>
        <Input
          placeholder="Tìm kiếm vật tư..."
          prefix={<SearchOutlined />}
          className="w-64"
        />
      </div>

      <Table
        columns={columns}
        dataSource={mockData}
        rowKey="id"
        pagination={{ pageSize: 20 }}
        size="small"
      />
    </div>
  );
};
