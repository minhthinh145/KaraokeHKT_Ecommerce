import React from "react";
import { DatePicker } from "antd";
import { CalendarOutlined, CloseOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
const { RangePicker } = DatePicker;

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

  showDateRangeFilter?: boolean;
  dateRangeValue?: [string | null, string | null];
  onDateRangeChange?: (range: [string | null, string | null]) => void;
  dateRangePlaceholder?: [string, string]; //date
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

  showDateRangeFilter = false,
  dateRangeValue = [null, null],
  onDateRangeChange,
  dateRangePlaceholder = ["Từ ngày", "Đến ngày"],

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

        {showDateRangeFilter && (
          <div className="flex items-center gap-2">
            <div className="relative">
              <RangePicker
                value={
                  dateRangeValue && dateRangeValue[0] && dateRangeValue[1]
                    ? [dayjs(dateRangeValue[0]), dayjs(dateRangeValue[1])]
                    : [null, null]
                }
                onChange={(dates) => {
                  if (onDateRangeChange) {
                    if (dates && dates[0] && dates[1]) {
                      onDateRangeChange([
                        dates[0].format("YYYY-MM-DD"),
                        dates[1].format("YYYY-MM-DD"),
                      ]);
                    } else {
                      onDateRangeChange([null, null]);
                    }
                  }
                }}
                allowClear={{
                  clearIcon: (
                    <CloseOutlined className="text-gray-400 hover:text-gray-600" />
                  ),
                }}
                placeholder={dateRangePlaceholder || ["Từ ngày", "Đến ngày"]}
                suffixIcon={<CalendarOutlined className="text-blue-500" />}
                className="min-w-[280px] h-10 rounded-lg border-gray-300 hover:border-blue-400 focus:border-blue-500 shadow-sm"
                popupClassName="shadow-lg"
                format="DD/MM/YYYY"
                separator={
                  <span className="text-gray-400 mx-2 font-medium">đến</span>
                }
                style={{
                  borderRadius: "8px",
                }}
              />

              {/* Label cho date picker */}
              <div className="absolute -top-2 left-3 bg-white px-1 text-xs text-gray-500 font-medium">
                Lọc theo ngày
              </div>
            </div>

            {/* Quick filter buttons */}
            <div className="flex gap-1">
              <button
                onClick={() => {
                  const today = dayjs();
                  const startOfWeek = today.startOf("week");
                  onDateRangeChange?.([
                    startOfWeek.format("YYYY-MM-DD"),
                    today.format("YYYY-MM-DD"),
                  ]);
                }}
                className="px-3 py-1.5 text-xs font-medium text-blue-600 bg-blue-50 hover:bg-blue-100 rounded-md transition-colors duration-200"
              >
                Tuần này
              </button>
              <button
                onClick={() => {
                  const today = dayjs();
                  const startOfMonth = today.startOf("month");
                  onDateRangeChange?.([
                    startOfMonth.format("YYYY-MM-DD"),
                    today.format("YYYY-MM-DD"),
                  ]);
                }}
                className="px-3 py-1.5 text-xs font-medium text-green-600 bg-green-50 hover:bg-green-100 rounded-md transition-colors duration-200"
              >
                Tháng này
              </button>
              <button
                onClick={() => {
                  onDateRangeChange?.([null, null]);
                }}
                className="px-3 py-1.5 text-xs font-medium text-gray-600 bg-gray-50 hover:bg-gray-100 rounded-md transition-colors duration-200"
              >
                Tất cả
              </button>
            </div>
          </div>
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
