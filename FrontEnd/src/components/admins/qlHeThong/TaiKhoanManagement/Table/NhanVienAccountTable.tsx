import React from "react";
import type { NhanVienTaiKhoanDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";
import { renderRoleBadge } from "../../../uiForAll/roleMacros";

interface NhanVienAccountTableProps {
  data: NhanVienTaiKhoanDTO[];
  loading: boolean;
  onLockToggle: (maTaiKhoan: string, isLocked: boolean) => void;
}

export const NhanVienAccountTable: React.FC<NhanVienAccountTableProps> = ({
  data,
  loading,
  onLockToggle,
}) => {
  const nhanVienColumns = [
    {
      key: "userName",
      title: "Tên đăng nhập",
      dataIndex: "userName" as keyof NhanVienTaiKhoanDTO,
      width: 180,
      render: (value: string) => TableHelpers.getUserNameCell(value),
      className:
        "font-semibold text-gray-900 group-hover:text-blue-600 transition-colors",
    },
    {
      key: "fullName",
      title: "Họ tên đầy đủ",
      dataIndex: "fullName" as keyof NhanVienTaiKhoanDTO,
      width: 200,
      className:
        "font-semibold text-gray-900 group-hover:text-indigo-600 transition-colors",
    },
    {
      key: "loaiTaiKhoan",
      title: "Vai trò",
      dataIndex: "loaiTaiKhoan" as keyof NhanVienTaiKhoanDTO,
      width: 160,
      render: renderRoleBadge,
    },
    {
      key: "trangThai",
      title: "Trạng thái",
      width: 140,
      render: (_: any, record: NhanVienTaiKhoanDTO) =>
        TableHelpers.getStatusBadge(record.daKichHoat, record.daBiKhoa),
    },
    {
      key: "emailConfirmed",
      title: "Email xác nhận",
      dataIndex: "emailConfirmed" as keyof NhanVienTaiKhoanDTO,
      width: 160,
      render: (value: boolean) => TableHelpers.getEmailConfirmedBadge(value),
    },
  ];

  return (
    <GenericQLTable
      data={data}
      loading={loading}
      columns={nhanVienColumns}
      rowKey="maTaiKhoan"
      onLockToggle={onLockToggle}
      showLockActions={true}
      lockStatusField="daBiKhoa"
      emptyMessage="Không có tài khoản nhân viên"
      tableName="tài khoản nhân viên"
    />
  );
};
