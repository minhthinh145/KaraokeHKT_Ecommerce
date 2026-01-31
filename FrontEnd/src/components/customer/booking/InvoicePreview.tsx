import React from "react";
import { Descriptions, Tag, Table, Button, Space, Alert } from "antd";
import type { TaoHoaDonPhongResponseDTO } from "../../../api/customer/bookingApi";
import dayjs from "dayjs";

interface Props {
  invoice: TaoHoaDonPhongResponseDTO;
  onConfirm: () => void; // giữ kiểu không tham số
  loading?: boolean;
}

export const InvoicePreview: React.FC<Props> = ({
  invoice,
  onConfirm,
  loading,
}) => {
  const items = invoice.hoaDonDetail.chiTietItems.map((c, i) => ({
    key: i,
    tenSanPham: c.tenSanPham,
    soLuong: c.soLuong,
    gia: c.giaPhong.toLocaleString(),
    thanhTien: c.thanhTien.toLocaleString(),
  }));

  return (
    <div className="space-y-4">
      <Alert
        type="info"
        showIcon
        message="Vui lòng kiểm tra thông tin trước khi xác nhận thanh toán (hạn 15 phút)."
      />
      <Descriptions bordered size="small" column={1}>
        <Descriptions.Item label="Phòng">{invoice.tenPhong}</Descriptions.Item>
        <Descriptions.Item label="Khách hàng">
          {invoice.tenKhachHang}
        </Descriptions.Item>
        <Descriptions.Item label="Bắt đầu">
          {dayjs(invoice.thoiGianBatDau).format("DD/MM/YYYY HH:mm")}
        </Descriptions.Item>
        <Descriptions.Item label="Kết thúc dự kiến">
          {dayjs(invoice.thoiGianKetThucDuKien).format("DD/MM/YYYY HH:mm")}
        </Descriptions.Item>
        <Descriptions.Item label="Số giờ">
          {invoice.soGioSuDung}
        </Descriptions.Item>
        <Descriptions.Item label="Giá/giờ">
          {invoice.giaPhong.toLocaleString()} đ
        </Descriptions.Item>
        <Descriptions.Item label="Tổng tiền">
          <b className="text-red-600">{invoice.tongTien.toLocaleString()} đ</b>
        </Descriptions.Item>
        <Descriptions.Item label="Hạn thanh toán">
          <Tag color="orange">
            {dayjs(invoice.hanThanhToan).format("HH:mm DD/MM/YYYY")}
          </Tag>
        </Descriptions.Item>
        {invoice.ghiChu && (
          <Descriptions.Item label="Ghi chú">
            {invoice.ghiChu}
          </Descriptions.Item>
        )}
      </Descriptions>
      <Table
        size="small"
        pagination={false}
        dataSource={items}
        columns={[
          { title: "Sản phẩm", dataIndex: "tenSanPham" },
          { title: "SL", dataIndex: "soLuong", width: 70 },
          { title: "Giá", dataIndex: "gia" },
          { title: "Thành tiền", dataIndex: "thanhTien" },
        ]}
      />
      <Space className="w-full flex justify-end">
        <Button type="primary" onClick={onConfirm} loading={loading}>
          Xác nhận & Thanh toán
        </Button>
      </Space>
    </div>
  );
};
