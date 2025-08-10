import React from "react";

export interface RoleOption {
  value: string;
  label: string;
}

interface AccountFilterBarProps {
  // Search
  searchPlaceholder?: string;
  searchValue: string;
  onSearchChange: (val: string) => void;

  // Role filter (optional)
  showRoleFilter?: boolean;
  roleValue?: string;
  roleOptions?: RoleOption[];
  onRoleChange?: (val: string) => void;
  roleAllLabel?: string;

  // Status filter (optional)
  showStatusFilter?: boolean;
  statusValue?: string;
  onStatusChange?: (val: string) => void;
  statusAllLabel?: string;

  // Actions
  onRefresh?: () => void;
  onClearAll?: () => void;
  refreshing?: boolean;

  // Extra area (optional custom actions)
  extraRight?: React.ReactNode;

  dense?: boolean; // nếu muốn thu nhỏ
  colorTheme?: "blue" | "green" | "gray";
}

const themeRing: Record<string, string> = {
  blue: "focus:ring-blue-500 focus:border-blue-500",
  green: "focus:ring-green-500 focus:border-green-500",
  gray: "focus:ring-gray-500 focus:border-gray-500",
};

export const AccountFilterBar: React.FC<AccountFilterBarProps> = ({
  searchPlaceholder = "Tìm kiếm...",
  searchValue,
  onSearchChange,

  showRoleFilter = false,
  roleValue = "",
  roleOptions = [],
  onRoleChange,
  roleAllLabel = "Tất cả vai trò",

  showStatusFilter = false,
  statusValue = "",
  onStatusChange,
  statusAllLabel = "Tất cả trạng thái",

  onRefresh,
  onClearAll,
  refreshing = false,

  extraRight,
  dense = false,
  colorTheme = "blue",
}) => {
  const spacing = dense ? "gap-3" : "gap-4";
  const pad = dense ? "px-2 py-1.5" : "px-3 py-2";
  const ring = themeRing[colorTheme] || themeRing.blue;

  return (
    <div className="bg-white p-4 rounded-lg border border-neutral-200">
      <div className={`flex flex-wrap items-center ${spacing}`}>
        {/* Search */}
        <div className="flex-1 min-w-[220px]">
          <input
            type="text"
            placeholder={searchPlaceholder}
            value={searchValue}
            onChange={(e) => onSearchChange(e.target.value)}
            className={`w-full ${pad} border border-gray-300 rounded-lg ${ring}`}
          />
        </div>

        {showRoleFilter && (
          <select
            value={roleValue}
            onChange={(e) => onRoleChange && onRoleChange(e.target.value)}
            className={`${pad} border border-gray-300 rounded-lg ${ring}`}
          >
            <option value="">{roleAllLabel}</option>
            {roleOptions.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        )}

        {showStatusFilter && (
          <select
            value={statusValue}
            onChange={(e) => onStatusChange && onStatusChange(e.target.value)}
            className={`${pad} border border-gray-300 rounded-lg ${ring}`}
          >
            <option value="">{statusAllLabel}</option>
            <option value="active">Hoạt động</option>
            <option value="inactive">Chưa kích hoạt</option>
            <option value="locked">Bị khóa</option>
          </select>
        )}

        {onRefresh && (
          <button
            onClick={onRefresh}
            disabled={refreshing}
            className={`${pad} bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors disabled:opacity-50`}
          >
            {refreshing ? "⏳" : "🔄"} Làm mới
          </button>
        )}

        {onClearAll && (
          <button
            onClick={onClearAll}
            className={`${pad} bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors`}
          >
            🗑️ Xóa lọc
          </button>
        )}

        {extraRight}
      </div>
    </div>
  );
};
