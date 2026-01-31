import React, { useState } from "react";
import { formatDateForDisplay } from "../../../api/services/admin/utils/dateUtils";
import { useConfirmDialog } from "../../../hooks/shared/useConfirmDialog";
import { Space, Button } from "antd";

// Generic interface cho table configuration
interface TableColumn<T> {
  key: string;
  title: string;
  dataIndex?: keyof T;
  width?: number;
  render?: (value: any, record: T, index: number) => React.ReactNode;
  className?: string;
}

export interface GenericQLTableProps<T> {
  data: T[];
  loading: boolean;
  columns: TableColumn<T>[];
  rowKey: keyof T;
  onLockToggle?: (id: string, isLocked: boolean) => void;
  onDelete?: (id: string) => Promise<{ success: boolean }>;
  showLockActions?: boolean;
  showDeleteAction?: boolean;
  titleDeleteAction?: string;
  messageDeleteAction?: string;
  lockStatusField?: keyof T;
  emptyMessage?: string;
  tableName?: string;
  onUpdate?: (row: T) => void;
  showUpdateAction?: boolean;
  extraRowActions?: (row: T) => React.ReactNode;
  onToggleNghiViec?: (row: T, next: boolean) => void | Promise<void>;
  showNghiViecAction?: boolean; // 🔥 thêm
}

export function GenericQLTable<T extends Record<string, any>>({
  data,
  loading,
  columns,
  rowKey,
  onLockToggle,
  onDelete,
  showLockActions = false,
  showDeleteAction = false,
  titleDeleteAction = "Xóa mục này?",
  messageDeleteAction = "Bạn có chắc chắn muốn xóa mục này? Hành động này không thể hoàn tác.",
  lockStatusField,
  emptyMessage = "Không có dữ liệu",
  tableName = "mục",
  onUpdate,
  showUpdateAction = false,
  extraRowActions,
  onToggleNghiViec,
  showNghiViecAction = false, // 🔥 thêm
}: GenericQLTableProps<T>) {
  const { showConfirm, ConfirmDialogComponent } = useConfirmDialog();

  const [loadingStates, setLoadingStates] = useState<Record<string, boolean>>(
    {}
  );
  const [deleteLoadingStates, setDeleteLoadingStates] = useState<
    Record<string, boolean>
  >({}); // 🔥 Loading cho delete

  const handleLockToggle = async (id: string, isLocked: boolean) => {
    if (!onLockToggle) return;

    setLoadingStates((prev) => ({ ...prev, [id]: true }));
    try {
      await onLockToggle(id, isLocked);
    } finally {
      setTimeout(() => {
        setLoadingStates((prev) => ({ ...prev, [id]: false }));
      }, 500);
    }
  };

  // Handle delete
  const handleDelete = async (id: string) => {
    if (!onDelete) return;

    setDeleteLoadingStates((prev) => ({ ...prev, [id]: true }));
    try {
      await onDelete(id);
    } finally {
      setTimeout(() => {
        setDeleteLoadingStates((prev) => ({ ...prev, [id]: false }));
      }, 500);
    }
  };

  // Common badge renderers
  const getStatusBadge = (daKichHoat: boolean, daBiKhoa: boolean) => {
    if (daBiKhoa) {
      return (
        <span className="inline-flex items-center px-3 py-1 text-xs font-bold rounded-full bg-gradient-to-r from-red-100 to-red-200 text-red-800 shadow-sm border border-red-300 animate-pulse">
          🔒 Bị khóa
        </span>
      );
    }
    if (daKichHoat) {
      return (
        <span className="inline-flex items-center px-3 py-1 text-xs font-bold rounded-full bg-gradient-to-r from-green-100 to-green-200 text-green-800 shadow-sm border border-green-300">
          ✅ Hoạt động
        </span>
      );
    }
    return (
      <span className="inline-flex items-center px-3 py-1 text-xs font-bold rounded-full bg-gradient-to-r from-yellow-100 to-orange-200 text-orange-800 shadow-sm border border-orange-300">
        ⏳ Chưa kích hoạt
      </span>
    );
  };

  const getEmailConfirmedBadge = (confirmed: boolean) => (
    <div className="flex items-center justify-center">
      {confirmed ? (
        <div className="flex items-center space-x-1 text-green-600">
          <svg
            className="w-5 h-5"
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
          <span className="text-xs font-semibold">Đã xác thực</span>
        </div>
      ) : (
        <div className="flex items-center space-x-1 text-red-600">
          <svg
            className="w-5 h-5 animate-pulse"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
          <span className="text-xs font-semibold">Chưa xác thực</span>
        </div>
      )}
    </div>
  );

  const getUserNameCell = (userName: string) => (
    <div className="flex items-center justify-center space-x-2">
      <svg
        className="w-5 h-5 text-blue-500"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth="2"
          d="M5.121 17.804A13.937 13.937 0 0112 15c2.5 0 4.847.655 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0z"
        />
      </svg>
      <span>{userName}</span>
    </div>
  );

  // 🎨 Sửa nút Lock/Unlock với màu đỏ/xanh
  const getLockActionButton = (record: T) => {
    if (!showLockActions || !onLockToggle || !lockStatusField) return null;

    const id = record[rowKey] as string;
    const isLocked = record[lockStatusField] as boolean;

    return (
      <div className="flex items-center justify-center">
        <button
          onClick={() => handleLockToggle(id, isLocked)}
          disabled={loadingStates[id]}
          className={`
            inline-flex items-center gap-2 px-3 py-2 text-sm font-medium 
            border rounded-lg transition-all duration-200 
            focus:outline-none focus:ring-2 focus:ring-offset-2
            disabled:opacity-50 disabled:cursor-not-allowed
            ${
              isLocked
                ? "text-green-700 bg-green-50 border-green-200 hover:bg-green-100 hover:border-green-300 focus:ring-green-500" // 🔥 Xanh cho mở khóa
                : "text-red-700 bg-red-50 border-red-200 hover:bg-red-100 hover:border-red-300 focus:ring-red-500" // 🔥 Đỏ cho khóa
            }
          `}
          title={isLocked ? "Mở khóa tài khoản" : "Khóa tài khoản"}
        >
          {loadingStates[id] ? (
            <div
              className={`w-4 h-4 border-2 border-current border-t-transparent rounded-full animate-spin`}
            ></div>
          ) : isLocked ? (
            // 🔥 Icon mở khóa - màu xanh
            <svg
              className="w-4 h-4 text-green-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M8 11V7a4 4 0 118 0m-4 8v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2z"
              />
            </svg>
          ) : (
            // 🔥 Icon khóa - màu đỏ
            <svg
              className="w-4 h-4 text-red-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"
              />
            </svg>
          )}
        </button>
      </div>
    );
  };

  // 🔥 Thêm nút Delete
  const getDeleteActionButton = (record: T) => {
    if (!showDeleteAction || !onDelete) return null;

    const id = record[rowKey] as string;

    return (
      <button
        onClick={() => {
          showConfirm(
            {
              title: titleDeleteAction,
              message: messageDeleteAction,
              confirmText: "Xóa",
              cancelText: "Hủy",
              variant: "danger",
            },
            () => handleDelete(id)
          );
        }}
        className="
          inline-flex items-center gap-2 px-3 py-2 text-sm font-medium 
          text-red-700 bg-red-50 border border-red-200 rounded-lg
          hover:bg-red-100 hover:border-red-300 
          focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500
          transition-all duration-200
        "
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
    );
  };

  // 🔥 Nút Update
  const getUpdateActionButton = (record: T) => {
    if (!showUpdateAction || !onUpdate) return null;
    return (
      <button
        onClick={() => onUpdate(record)}
        className="
          inline-flex items-center gap-2 px-3 py-2 text-sm font-medium
          text-indigo-700 bg-indigo-50 border border-indigo-200 rounded-lg
          hover:bg-indigo-100 hover:border-indigo-300
          focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500
          transition-all duration-200
        "
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
            d="M11 5h2m-1 0v14m9-7H3"
          />
        </svg>
        <span>Sửa</span>
      </button>
    );
  };

  // 🔥 Nút Nghỉ việc/Trở lại làm việc (kèm confirm)
  const getNghiViecActionButton = (record: T) => {
    if (!showNghiViecAction || !onToggleNghiViec) return null;
    if (typeof record.daNghiViec === "undefined") return null;

    const isDaNghi = Boolean(record.daNghiViec);
    const title = isDaNghi ? "Trở lại làm việc?" : "Cho nghỉ việc?";
    const message = isDaNghi
      ? "Xác nhận mở khóa tài khoản và đánh dấu nhân viên đang làm việc?"
      : "Xác nhận khóa tài khoản và đánh dấu nhân viên đã nghỉ việc?";

    return (
      <Button
        size="small"
        danger={!isDaNghi}
        onClick={() =>
          showConfirm(
            {
              title,
              message,
              confirmText: isDaNghi ? "Trở lại" : "Cho nghỉ",
              cancelText: "Hủy",
              variant: isDaNghi ? "primary" : "danger",
            },
            () => onToggleNghiViec(record, !isDaNghi)
          )
        }
      >
        {isDaNghi ? "Trở lại làm việc" : "Cho nghỉ việc"}
      </Button>
    );
  };

  // 🔥 Render cột thao tác với cả lock và delete
  const getActionButtons = (record: T) => {
    const lockButton = getLockActionButton(record);
    const deleteButton = getDeleteActionButton(record);
    const updateButton = getUpdateActionButton(record);
    const nghiViecButton = getNghiViecActionButton(record); // 🔥 thêm
    const extra = extraRowActions ? extraRowActions(record) : null;

    if (
      !lockButton &&
      !deleteButton &&
      !updateButton &&
      !nghiViecButton &&
      !extra
    )
      return null;

    return (
      <div className="flex items-center justify-center gap-2">
        {lockButton}
        {deleteButton}
        {updateButton}
        {nghiViecButton} {/* 🔥 thêm */}
        {extra}
      </div>
    );
  };

  // Loading skeleton
  const LoadingSkeleton = () => (
    <div className="bg-white rounded-lg border border-neutral-200 shadow-sm">
      <div className="overflow-x-auto">
        <table className="w-full divide-y divide-gray-200">
          <thead className="bg-gradient-to-r from-gray-50 to-gray-100">
            <tr>
              {columns.map((_, i) => (
                <th key={i} className="px-6 py-4">
                  <div className="h-4 bg-gradient-to-r from-gray-200 to-gray-300 rounded animate-pulse"></div>
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {Array.from({ length: 5 }).map((_, i) => (
              <tr key={i} className="animate-pulse">
                {columns.map((_, j) => (
                  <td key={j} className="px-6 py-4">
                    <div className="h-4 bg-gradient-to-r from-gray-100 to-gray-200 rounded"></div>
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );

  if (loading) {
    return <LoadingSkeleton />;
  }

  if (data.length === 0) {
    return (
      <div className="bg-white rounded-lg border border-neutral-200 shadow-lg">
        <div className="flex items-center justify-center h-64">
          <div className="text-center">
            <div className="mx-auto h-24 w-24 bg-gradient-to-br from-blue-100 to-purple-100 rounded-full flex items-center justify-center mb-4 animate-bounce">
              <svg
                className="h-12 w-12 text-gray-400"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 48 48"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
                />
              </svg>
            </div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">
              {emptyMessage}
            </h3>
            <p className="text-gray-500">
              Không tìm thấy dữ liệu phù hợp với tiêu chí tìm kiếm.
            </p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <>
      <div className="bg-white rounded-xl border border-neutral-200 shadow-lg overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full divide-y divide-gray-200">
            <thead>
              <tr>
                {columns.map((column) => (
                  <th
                    key={column.key}
                    className="px-6 py-4 text-center text-xs font-bold text-gray-600 uppercase tracking-wider whitespace-nowrap border-b-2 border-blue-200"
                    style={{ width: column.width }}
                  >
                    {column.title}
                  </th>
                ))}
                {(showLockActions ||
                  showDeleteAction ||
                  showUpdateAction ||
                  showNghiViecAction || // 🔥 thêm
                  extraRowActions) && (
                  <th
                    className="px-6 py-4 text-center text-xs font-bold text-gray-600 uppercase tracking-wider whitespace-nowrap border-b-2 border-blue-200"
                    style={{ width: 140, minWidth: 110, maxWidth: 200 }}
                  >
                    Thao tác
                  </th>
                )}
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {data.map((record, rowIndex) => (
                <tr
                  key={record[rowKey] as string}
                  className="hover:bg-gradient-to-r hover:from-blue-25 hover:to-indigo-25 transition-all duration-300 transform hover:scale-[1.01] hover:shadow-md group"
                  style={{ animationDelay: `${rowIndex * 50}ms` }}
                >
                  {columns.map((column) => (
                    <td
                      key={column.key}
                      className={`px-6 py-4 text-center whitespace-nowrap ${
                        column.className || ""
                      }`}
                    >
                      {column.render
                        ? column.render(
                            column.dataIndex
                              ? record[column.dataIndex]
                              : record,
                            record,
                            rowIndex
                          )
                        : column.dataIndex
                        ? record[column.dataIndex]
                        : ""}
                    </td>
                  ))}
                  {(showLockActions ||
                    showDeleteAction ||
                    showUpdateAction ||
                    showNghiViecAction || // 🔥 thêm
                    extraRowActions) && (
                    <td className="px-6 py-4 text-center whitespace-nowrap">
                      {getActionButtons(record)}
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Footer */}
        <div className="bg-gradient-to-r from-gray-50 to-blue-50 px-6 py-4 border-t border-gray-200">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <span className="text-sm font-semibold text-gray-700">
                Hiển thị{" "}
                <span className="font-bold text-blue-600 text-lg">
                  {data.length}
                </span>{" "}
                {tableName}
              </span>
            </div>
            <div className="flex items-center space-x-2 text-xs text-gray-500">
              <svg
                className="w-4 h-4 animate-bounce"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M7 16l-4-4m0 0l4-4m-4 4h18"
                />
              </svg>
              <span>Kéo ngang để xem thêm</span>
            </div>
          </div>
        </div>
      </div>

      {/* 🔥 Thêm ConfirmDialog */}
      {ConfirmDialogComponent}
    </>
  );
}

// Export helper functions để tái sử dụng
export const TableHelpers = {
  getStatusBadge: (daKichHoat: boolean, daBiKhoa: boolean) => {
    if (daBiKhoa) {
      return (
        <span className="inline-flex items-center px-3 py-1 text-xs font-bold rounded-full bg-gradient-to-r from-red-100 to-red-200 text-red-800 shadow-sm border border-red-300 animate-pulse">
          🔒 Bị khóa
        </span>
      );
    }
    if (daKichHoat) {
      return (
        <span className="inline-flex items-center px-3 py-1 text-xs font-bold rounded-full bg-gradient-to-r from-green-100 to-green-200 text-green-800 shadow-sm border border-green-300">
          ✅ Hoạt động
        </span>
      );
    }
    return (
      <span className="inline-flex items-center px-3 py-1 text-xs font-bold rounded-full bg-gradient-to-r from-yellow-100 to-orange-200 text-orange-800 shadow-sm border border-orange-300">
        ⏳ Chưa kích hoạt
      </span>
    );
  },

  getEmailConfirmedBadge: (confirmed: boolean) => (
    <div className="flex items-center justify-center">
      {confirmed ? (
        <div className="flex items-center space-x-1 text-green-600">
          <svg
            className="w-5 h-5"
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
          <span className="text-xs font-semibold">Đã xác thực</span>
        </div>
      ) : (
        <div className="flex items-center space-x-1 text-red-600">
          <svg
            className="w-5 h-5 animate-pulse"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
          <span className="text-xs font-semibold">Chưa xác thực</span>
        </div>
      )}
    </div>
  ),

  getUserNameCell: (userName: string) => (
    <div className="flex items-center justify-center space-x-2">
      <svg
        className="w-5 h-5 text-indigo-500"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth="2"
          d="M5.121 17.804A13.937 13.937 0 0112 15c2.5 0 4.847.655 6.879 1.804M15 10a3 3 0 11-6 0 3 3 0 016 0z"
        />
      </svg>
      <span
        className="
        font-bold text-indigo-500 bg-gradient-to-r from-indigo-50 to-indigo-100
        px-2 py-1 rounded-md shadow-sm tracking-wide
        group-hover:bg-indigo-200  transition-all
      "
        style={{ fontSize: "1.05em" }}
      >
        {userName}
      </span>
    </div>
  ),
  getCodeCell: (code: string) => (
    <div className="bg-gradient-to-r from-gray-100 to-gray-200 px-2 py-1 rounded-md">
      {code.substring(0, 8)}...
    </div>
  ),

  getDateCell: (date: string | null) =>
    date ? formatDateForDisplay(date) : "-",
};
