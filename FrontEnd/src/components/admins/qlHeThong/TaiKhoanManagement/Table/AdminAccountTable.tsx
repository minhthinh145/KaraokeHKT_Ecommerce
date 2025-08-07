import React from "react";
import type { AdminAccountDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";
import { renderRoleBadge } from "../../../uiForAll/roleMacros";

interface AdminAccountTableProps {
  data: AdminAccountDTO[];
  loading: boolean;
}

export const AdminAccountTable: React.FC<AdminAccountTableProps> = ({
  data,
  loading,
}) => {
  const AdminColumns = [
    {
      key: "maTaiKhoan",
      title: "Mã TK",
      dataIndex: "maTaiKhoan" as keyof AdminAccountDTO,
      render: (value: string) => TableHelpers.getCodeCell(value),
      className: "font-mono text-gray-900 group-hover:font-bold transition-all",
    },
    {
      key: "userName",
      title: "Tên đăng nhập",
      dataIndex: "userName" as keyof AdminAccountDTO,
      render: (value: string) => TableHelpers.getUserNameCell(value),
      className:
        "font-semibold text-gray-900 group-hover:text-blue-600 transition-colors",
    },
    {
      key: "email",
      title: "Email đăng nhập",
      dataIndex: "email" as keyof AdminAccountDTO,
      className:
        "font-semibold text-gray-900 group-hover:text-indigo-600 transition-colors",
    },
    {
      key: "loaiTaiKhoan",
      title: "Vai trò",
      dataIndex: "loaiTaiKhoan" as keyof AdminAccountDTO,
      render: renderRoleBadge,
    },
    {
      key: "trangThai",
      title: "Trạng thái",
      render: (_: any, record: AdminAccountDTO) =>
        TableHelpers.getStatusBadge(record.daKichHoat, record.daBiKhoa),
    },
  ];

  return (
    <GenericQLTable
      data={data}
      loading={loading}
      columns={AdminColumns}
      rowKey="maTaiKhoan"
      showLockActions={false}
      emptyMessage="Không có tài khoản quản trị nào"
      tableName="Tài khoản quản trị"
    />
  );
};
