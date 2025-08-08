import React from "react";

import type { KhachHangTaiKhoanDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";

interface KhacHangAccountTableProps {
  data: KhachHangTaiKhoanDTO[];
  loading: boolean;
  onLockToggle: (maTaiKhoan: string, isLocked: boolean) => void;
}

export const KhachHangAccountTable: React.FC<KhacHangAccountTableProps> = ({
  data,
  loading,
  onLockToggle,
}) => {
  const khachHangColumns = [
    {
      key: "maTaiKhoan",
      title: "Mã TK",
      dataIndex: "maTaiKhoan" as keyof KhachHangTaiKhoanDTO,
      width: 120,
      render: (value: string) => TableHelpers.getCodeCell(value),
      className: "font-mono text-gray-900 group-hover:font-bold transition-all",
    },
    {
      key: "userName",
      title: "Email đăng nhập",
      dataIndex: "userName" as keyof KhachHangTaiKhoanDTO,
      width: 180,
      render: (value: string) => TableHelpers.getUserNameCell(value),
      className:
        "font-semibold text-gray-900 group-hover:text-blue-600 transition-colors",
    },
    {
      key: "tenKhachHang",
      title: "Tên khách hàng",
      dataIndex: "tenKhachHang" as keyof KhachHangTaiKhoanDTO,
      width: 250,
      className: "text-gray-700 group-hover:text-gray-900 transition-colors",
    },
    {
      key: "trangThai",
      title: "Trạng thái",
      width: 140,
      render: (_: any, record: KhachHangTaiKhoanDTO) =>
        TableHelpers.getStatusBadge(record.daKichHoat, record.daBiKhoa),
    },
    {
      key: "emailConfirmed",
      title: "Email xác thực",
      dataIndex: "emailConfirmed" as keyof KhachHangTaiKhoanDTO,
      width: 160,
      render: (value: boolean) => TableHelpers.getEmailConfirmedBadge(value),
    },
  ];
  return (
    <GenericQLTable
      data={data}
      loading={loading}
      columns={khachHangColumns}
      rowKey="maTaiKhoan"
      onLockToggle={onLockToggle}
      showLockActions={true}
      lockStatusField="daBiKhoa"
      emptyMessage="Không có tài khoản khách hàng nào phù hợp với tiêu chí tìm kiếm."
      tableName="tài khoản khách hàng"
    />
  );
};
