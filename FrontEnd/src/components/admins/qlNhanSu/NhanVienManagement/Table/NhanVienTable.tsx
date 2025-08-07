import React from "react";
import { formatDateForDisplay } from "../../../../../api/services/admin/utils/dateUtils";
import type { NhanVienDTO } from "../../../../../api";
import { GenericQLTable, TableHelpers } from "../../../uiForAll/GenericQLTable";
import { renderRoleBadge } from "../../../uiForAll/roleMacros";

interface NhanVienTableProps {
  data: NhanVienDTO[];
  loading: boolean;
  onEdit: (nhanVien: NhanVienDTO) => void;
  onDelete: (maNv: string) => void;
}

export const NhanVienTable: React.FC<NhanVienTableProps> = ({
  data,
  loading,
  onEdit,
  onDelete,
}) => {
  const nhanVienColumns = [
    {
      key: "maNv",
      title: "Mã NV",
      dataIndex: "maNv" as keyof NhanVienDTO,
      width: 120,
      render: (value: string) => TableHelpers.getCodeCell(value),
      className: "font-mono text-gray-900 group-hover:font-bold transition-all",
    },
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
    {
      key: "actions",
      title: "Thao tác",
      width: 120,
      render: (_: any, record: NhanVienDTO) => (
        <div className="flex items-center justify-center gap-2">
          <button
            onClick={() => onEdit(record)}
            className="text-indigo-600 hover:text-indigo-900 transition-colors p-1 rounded hover:bg-indigo-50"
            title="Sửa"
          >
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
              />
            </svg>
          </button>
          <button
            onClick={() => onDelete(record.maNv)}
            className="text-red-600 hover:text-red-900 transition-colors p-1 rounded hover:bg-red-50"
            title="Xóa"
          >
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
              />
            </svg>
          </button>
        </div>
      ),
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
      emptyMessage="Không tìm thấy nhân viên nào phù hợp với tiêu chí tìm kiếm"
      tableName="nhân viên"
    />
  );
};
