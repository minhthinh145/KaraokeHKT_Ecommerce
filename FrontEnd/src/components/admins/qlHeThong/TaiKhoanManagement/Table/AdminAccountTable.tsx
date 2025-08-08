import React from "react";
import type { AdminAccountDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";
import { renderRoleBadge } from "../../../uiForAll/roleMacros";

interface AdminAccountTableProps {
  data: AdminAccountDTO[];
  loading: boolean;

  onLockToggle: (maTaiKhoan: string, isLocked: boolean) => void;
  onDelete: (maTaiKhoan: string) => Promise<{ success: boolean }>;
}

export const AdminAccountTable: React.FC<AdminAccountTableProps> = ({
  data,
  loading,
  onLockToggle,
  onDelete,
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
      dataIndex: "email" as keyof AdminAccountDTO,
      render: (value: string) => TableHelpers.getUserNameCell(value),
      className:
        "font-semibold text-gray-900 group-hover:text-blue-600 transition-colors",
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
      onLockToggle={onLockToggle}
      onDelete={onDelete}
      showLockActions={true}
      showDeleteAction={true}
      lockStatusField="daBiKhoa"
      emptyMessage="Không có tài khoản quản trị nào"
      tableName="Tài khoản quản trị"
    />
  );
};
