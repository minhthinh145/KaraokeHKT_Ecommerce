import React from "react";
import { formatDateForDisplay } from "../../../../../api/services/admin/utils/dateUtils";
import type { NhanVienDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";
import { renderRoleBadge } from "../../../uiForAll/roleMacros";

interface NhanVienTableProps {
  data: NhanVienDTO[];
  loading: boolean;
  onUpdate: (nhanVien: NhanVienDTO) => void;
  onDelete: (maNv: string) => Promise<{ success: boolean }>;
}

export const NhanVienTable: React.FC<NhanVienTableProps> = ({
  data,
  loading,
  onUpdate,
  onDelete,
}) => {
  const nhanVienColumns = [
    {
      key: "hoTen",
      title: "Họ tên",
      dataIndex: "hoTen" as keyof NhanVienDTO,
      width: 180,
      className:
        "font-semibold text-gray-900 group-hover:text-blue-600 transition-colors",
    },
    {
      key: "email",
      title: "Email",
      dataIndex: "email" as keyof NhanVienDTO,
      width: 220,
      className: "text-gray-700 group-hover:text-gray-900 transition-colors",
    },
    {
      key: "soDienThoai",
      title: "SĐT",
      dataIndex: "soDienThoai" as keyof NhanVienDTO,
      width: 120,
      render: (value: string) => value || "-",
      className: "text-gray-700",
    },
    {
      key: "ngaySinh",
      title: "Ngày sinh",
      dataIndex: "ngaySinh" as keyof NhanVienDTO,
      width: 120,
      render: (value: string) => formatDateForDisplay(value),
      className: "text-gray-700",
    },
    {
      key: "loaiNhanVien",
      title: "Loại nhân viên",
      dataIndex: "loaiNhanVien" as keyof NhanVienDTO,
      width: 160,
      render: renderRoleBadge,
    },
  ];

  if (loading) {
    return (
      <div className="bg-white rounded-lg border border-neutral-200">
        <div className="flex items-center justify-center h-64">
          <div className="flex items-center gap-3">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-indigo-600"></div>
            <span className="text-gray-600">Đang tải dữ liệu...</span>
          </div>
        </div>
      </div>
    );
  }

  if (data.length === 0) {
    return (
      <div className="bg-white rounded-lg border border-neutral-200">
        <div className="flex items-center justify-center h-64">
          <div className="text-center">
            <svg
              className="mx-auto h-12 w-12 text-gray-400"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 48 48"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M34 40h10v-4a6 6 0 00-10.712-3.714M34 40H14m20 0v-4a9.971 9.971 0 00-.712-3.714M14 40H4v-4a6 6 0 0110.712-3.714M14 40v-4a9.971 9.971 0 01.712-3.714M34 20h-4M22 20h-4m12 0a4 4 0 01-8 0m8 0V8a4 4 0 10-8 0v12"
              />
            </svg>
            <h3 className="mt-2 text-sm font-medium text-gray-900">
              Không có nhân viên
            </h3>
            <p className="mt-1 text-sm text-gray-500">
              Không tìm thấy nhân viên nào phù hợp với tiêu chí tìm kiếm.
            </p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <GenericQLTable
      data={data}
      loading={loading}
      columns={nhanVienColumns}
      rowKey="maNv"
      showLockActions={false} // Nhân viên không cần lock/unlock
      showDeleteAction={true}
      onDelete={onDelete}
      showUpdateAction={true}
      onUpdate={onUpdate}
      emptyMessage="Không tìm thấy nhân viên nào phù hợp với tiêu chí tìm kiếm"
      tableName="nhân viên"
    />
  );
};
