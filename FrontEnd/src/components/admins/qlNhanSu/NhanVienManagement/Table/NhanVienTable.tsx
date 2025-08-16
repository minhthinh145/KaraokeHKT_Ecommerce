import React from "react";
import { formatDateForDisplay } from "../../../../../api/services/admin/utils/dateUtils";
import type { NhanVienDTO } from "../../../../../api";
import { GenericQLTable } from "../../../uiForAll/GenericQLTable";
import { renderRoleBadge } from "../../../uiForAll/roleMacros";
import { QLActionButton } from "../../../uiForAll/QLActionButton";

interface NhanVienTableProps {
  data: NhanVienDTO[];
  loading: boolean;
  onUpdate: (nhanVien: NhanVienDTO) => void;
  onToggleNghiViec: (nhanVien: NhanVienDTO, value: boolean) => void;
  // thêm cờ điều khiển action (mặc định giữ nguyên UI hiện tại)
  showUpdateAction?: boolean;
  showNghiViecAction?: boolean;
}

export const NhanVienTable: React.FC<NhanVienTableProps> = ({
  data,
  loading,
  onUpdate,
  onToggleNghiViec,
  showUpdateAction = true,
  showNghiViecAction = true,
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
    // Đã nghỉ việc: làm nổi bật bằng BG trong cell, không đổi layout
    {
      key: "daNghiViec",
      title: "Đã nghỉ việc",
      dataIndex: "daNghiViec" as keyof NhanVienDTO,
      width: 120,
      className: "text-gray-700",
      render: (value: boolean | undefined) => (
        <span
          className={[
            "block w-full px-2 py-1 rounded text-xs font-medium border text-center",
            value
              ? "bg-red-50 text-red-700 border-red-200"
              : "bg-emerald-50 text-emerald-700 border-emerald-200",
          ].join(" ")}
        >
          {value ? "Đã nghỉ việc" : "Đang làm việc"}
        </span>
      ),
    },
  ];

  const extraRowActions = (row: NhanVienDTO) => (
    <div className="flex items-center justify-center gap-2">
      {row.daNghiViec ? (
        <QLActionButton
          onClick={() => onToggleNghiViec(row, false)}
          color="green"
          title="Khôi phục làm việc"
          icon={
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
                d="M5 13l4 4L19 7"
              />
            </svg>
          }
        >
          Khôi phục
        </QLActionButton>
      ) : (
        <QLActionButton
          onClick={() => onToggleNghiViec(row, true)}
          color="red"
          title="Nghỉ việc"
          icon={
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
                d="M18.364 5.636l-1.414 1.414M6.343 17.657l-1.415 1.415M5.636 5.636l1.414 1.414M17.657 17.657l1.415 1.415M12 8v4m0 4h.01"
              />
            </svg>
          }
        >
          Nghỉ việc
        </QLActionButton>
      )}
    </div>
  );

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
      showLockActions={false}
      showUpdateAction={showUpdateAction} // dùng cờ truyền vào
      onUpdate={onUpdate}
      emptyMessage="Không tìm thấy nhân viên nào phù hợp với tiêu chí tìm kiếm"
      tableName="nhân viên"
      showNghiViecAction={showNghiViecAction} // dùng cờ truyền vào
      extraRowActions={extraRowActions}
    />
  );
};
