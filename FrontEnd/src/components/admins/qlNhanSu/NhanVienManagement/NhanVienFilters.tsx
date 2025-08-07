import React from "react";

interface FilterOption {
  value: string;
  label: string;
}

interface NhanVienFiltersProps {
  searchQuery: string;
  onSearchChange: (query: string) => void;
  loaiNhanVienFilter: string;
  onLoaiNhanVienChange: (type: string) => void;
  filterOptions: FilterOption[];
  onClearFilters: () => void;
  totalCount: number;
  filteredCount: number;
}

export const NhanVienFilters: React.FC<NhanVienFiltersProps> = ({
  searchQuery,
  onSearchChange,
  loaiNhanVienFilter,
  onLoaiNhanVienChange,
  filterOptions,
  onClearFilters,
  totalCount,
  filteredCount,
}) => {
  return (
    <div className="bg-white p-6 rounded-lg border border-neutral-200 mb-6">
      <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4">
        {/* 🔍 Search */}
        <div className="flex-1 max-w-md">
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg
                className="h-5 w-5 text-gray-400"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                />
              </svg>
            </div>
            <input
              type="text"
              placeholder="Tìm kiếm theo tên, email, SĐT..."
              value={searchQuery}
              onChange={(e) => onSearchChange(e.target.value)}
              className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
            />
          </div>
        </div>

        {/* 🔽 Filters */}
        <div className="flex flex-col sm:flex-row gap-4">
          {/* Loại nhân viên filter */}
          <div className="min-w-[200px]">
            <select
              value={loaiNhanVienFilter}
              onChange={(e) => onLoaiNhanVienChange(e.target.value)}
              className="block w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
            >
              {filterOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          {/* Clear filters */}
          <button
            onClick={onClearFilters}
            disabled={searchQuery === "" && loaiNhanVienFilter === "All"}
            className="px-4 py-2 text-sm text-gray-600 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Xóa bộ lọc
          </button>
        </div>
      </div>

      {/* 📊 Results count */}
      <div className="mt-4 flex items-center justify-between text-sm text-gray-600">
        <div>
          Hiển thị{" "}
          <span className="font-medium text-gray-900">{filteredCount}</span>{" "}
          trên <span className="font-medium text-gray-900">{totalCount}</span>{" "}
          nhân viên
        </div>

        {(searchQuery || loaiNhanVienFilter !== "All") && (
          <div className="text-indigo-600">🔍 Đang áp dụng bộ lọc</div>
        )}
      </div>
    </div>
  );
};
