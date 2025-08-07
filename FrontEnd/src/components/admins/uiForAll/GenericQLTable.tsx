import React, { useState } from "react";
import { formatDateForDisplay } from "../../../api/services/admin/utils/dateUtils";

// Generic interface cho table configuration
interface TableColumn<T> {
  key: string;
  title: string;
  dataIndex?: keyof T;
  width?: number;
  render?: (value: any, record: T, index: number) => React.ReactNode;
  className?: string;
}

interface GenericQLTableProps<T> {
  data: T[];
  loading: boolean;
  columns: TableColumn<T>[];
  rowKey: keyof T;
  onLockToggle?: (id: string, isLocked: boolean) => void;
  showLockActions?: boolean;
  lockStatusField?: keyof T; // field để check trạng thái khóa
  emptyMessage?: string;
  tableName?: string;
}

export function GenericQLTable<T extends Record<string, any>>({
  data,
  loading,
  columns,
  rowKey,
  onLockToggle,
  showLockActions = false,
  lockStatusField,
  emptyMessage = "Không có dữ liệu",
  tableName = "mục",
}: GenericQLTableProps<T>) {
  const [loadingStates, setLoadingStates] = useState<Record<string, boolean>>(
    {}
  );

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

  const getLockActionButton = (record: T) => {
    if (!showLockActions || !onLockToggle || !lockStatusField) return null;

    const id = record[rowKey] as string;
    const isLocked = record[lockStatusField] as boolean;

    return (
      <div className="flex items-center justify-center">
        {isLocked ? (
          <button
            onClick={() => handleLockToggle(id, true)}
            disabled={loadingStates[id]}
            className="inline-flex items-center gap-2 px-4 py-2 text-sm font-bold text-white bg-gradient-to-r from-green-500 to-emerald-600 border border-green-600 rounded-xl hover:from-green-600 hover:to-emerald-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500 transition-all duration-300 shadow-lg hover:shadow-xl transform hover:scale-105 disabled:opacity-70 disabled:cursor-not-allowed"
            title="Mở khóa tài khoản"
          >
            {loadingStates[id] ? (
              <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
            ) : (
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
                  d="M3 11V7a5 5 0 0110 0v4M5 19h14a2 2 0 002-2v-5a2 2 0 00-2-2H5a2 2 0 00-2 2v5a2 2 0 002 2z"
                />
              </svg>
            )}
          </button>
        ) : (
          <button
            onClick={() => handleLockToggle(id, false)}
            disabled={loadingStates[id]}
            className="inline-flex items-center gap-2 px-4 py-2 text-sm font-bold text-white bg-gradient-to-r from-red-500 to-rose-600 border border-red-600 rounded-xl hover:from-red-600 hover:to-rose-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 transition-all duration-300 shadow-lg hover:shadow-xl transform hover:scale-105 disabled:opacity-70 disabled:cursor-not-allowed"
            title="Khóa tài khoản"
          >
            {loadingStates[id] ? (
              <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
            ) : (
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
                  d="M12 17a2 2 0 100-4 2 2 0 000 4zm6-6V7a6 6 0 10-12 0v4"
                />
                <rect width="20" height="8" x="2" y="11" rx="2" />
              </svg>
            )}
          </button>
        )}
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
    <div className="bg-white rounded-xl border border-neutral-200 shadow-lg overflow-hidden">
      <div className="overflow-x-auto">
        <table className="w-full divide-y divide-gray-200">
          <thead className="bg-gradient-to-r from-slate-50 via-blue-50 to-indigo-50 sticky top-0 z-10">
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
              {showLockActions && (
                <th className="px-6 py-4 text-center text-xs font-bold text-gray-600 uppercase tracking-wider whitespace-nowrap border-b-2 border-blue-200">
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
                          column.dataIndex ? record[column.dataIndex] : record,
                          record,
                          rowIndex
                        )
                      : column.dataIndex
                      ? record[column.dataIndex]
                      : ""}
                  </td>
                ))}
                {showLockActions && (
                  <td className="px-6 py-4 text-center whitespace-nowrap">
                    {getLockActionButton(record)}
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
  ),

  getCodeCell: (code: string) => (
    <div className="bg-gradient-to-r from-gray-100 to-gray-200 px-2 py-1 rounded-md">
      {code.substring(0, 8)}...
    </div>
  ),

  getDateCell: (date: string | null) =>
    date ? formatDateForDisplay(date) : "-",
};
