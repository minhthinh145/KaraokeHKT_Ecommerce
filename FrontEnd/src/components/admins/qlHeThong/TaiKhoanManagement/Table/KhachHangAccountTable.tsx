import React from "react";

import type { KhachHangTaiKhoanDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";

interface KhacHangAccountTableProps {
  data: KhachHangTaiKhoanDTO[];
  loading: boolean;
}

export const KhachHangAccountTable: React.FC<KhacHangAccountTableProps> = ({
  data,
  loading,
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
      title: "Tên đăng nhập",
      dataIndex: "userName" as keyof KhachHangTaiKhoanDTO,
      width: 180,
      render: (value: string) => TableHelpers.getUserNameCell(value),
      className:
        "font-semibold text-gray-900 group-hover:text-blue-600 transition-colors",
    },
    {
      key: "email",
      title: "Email",
      dataIndex: "email" as keyof KhachHangTaiKhoanDTO,
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
      showLockActions={false}
      emptyMessage="Không có tài khoản khách hàng nào phù hợp với tiêu chí tìm kiếm."
      tableName="tài khoản khách hàng"
    />
  );
};
