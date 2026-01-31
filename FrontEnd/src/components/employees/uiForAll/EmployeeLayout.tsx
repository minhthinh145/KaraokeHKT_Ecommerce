import React, { useState } from "react";
import {
  MagnifyingGlassIcon,
  FunnelIcon,
  Bars3Icon,
  XMarkIcon,
  ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/outline";
import { useAuth } from "../../../hooks/auth/useAuth";

interface EmployeeLayoutProps {
  children: React.ReactNode;
  sidebarContent: React.ReactNode;
  pageTitle?: string;
  searchPlaceholder?: string;
  showSearch?: boolean;
  showFilter?: boolean;
  onSearchChange?: (q: string) => void;
}

export const EmployeeLayout: React.FC<EmployeeLayoutProps> = ({
  children,
  sidebarContent,
  pageTitle = "Nhân viên",
  searchPlaceholder = "Tìm kiếm...",
  showSearch = false,
  showFilter = false,
  onSearchChange,
}) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const { logout, user } = useAuth();

  const handleSearch = (v: string) => {
    setSearchQuery(v);
    onSearchChange?.(v);
  };

  const displayName =
    user?.profile?.userName ||
    user?.profile?.userName ||
    user?.loaiTaiKhoan ||
    "Nhân viên";

  return (
    <div className="h-screen w-screen bg-white flex overflow-hidden border border-neutral-200">
      {/* Sidebar */}
      <div
        className={`${
          isSidebarOpen ? "w-72" : "w-20"
        } transition-all duration-300 bg-white border-r border-neutral-200 flex flex-col flex-shrink-0`}
      >
        <div className="p-5 border-b border-neutral-200 flex items-center justify-between">
          {isSidebarOpen && (
            <h1 className="text-lg font-bold font-['Space_Grotesk'] truncate">
              {pageTitle}
            </h1>
          )}
          <button
            onClick={() => setIsSidebarOpen((s) => !s)}
            className="p-1.5 rounded hover:bg-gray-100"
          >
            {isSidebarOpen ? (
              <XMarkIcon className="w-5 h-5" />
            ) : (
              <Bars3Icon className="w-5 h-5" />
            )}
          </button>
        </div>
        <div className="flex-1 overflow-y-auto">{sidebarContent}</div>
      </div>

      {/* Main */}
      <div className="flex-1 flex flex-col min-w-0">
        <header className="bg-white border-b border-neutral-200 px-6 py-4 flex items-center justify-between gap-6">
          {(showSearch || showFilter) && (
            <div className="flex items-center gap-4">
              {showSearch && (
                <div className="relative">
                  <MagnifyingGlassIcon className="absolute left-3 top-1/2 -translate-y-1/2 w-5 h-5 text-zinc-500" />
                  <input
                    value={searchQuery}
                    onChange={(e) => handleSearch(e.target.value)}
                    placeholder={searchPlaceholder}
                    className="w-80 pl-10 pr-3 py-2 rounded border border-neutral-300 text-sm focus:ring-2 focus:ring-indigo-500 outline-none"
                  />
                </div>
              )}
              {showFilter && (
                <button className="flex items-center gap-2 px-3 py-2 rounded border text-sm hover:bg-gray-50 border-neutral-300">
                  <FunnelIcon className="w-5 h-5 text-zinc-500" />
                  <span>Bộ lọc</span>
                </button>
              )}
            </div>
          )}
          <div className="flex items-center gap-3">
            <div className="w-8 h-8 rounded-full bg-indigo-500 text-white flex items-center justify-center text-sm font-semibold">
              {displayName.charAt(0).toUpperCase()}
            </div>
            <span className="text-sm font-medium text-gray-700 hidden sm:inline">
              {displayName}
            </span>
            <button
              onClick={logout}
              className="flex items-center gap-1.5 px-3 py-1.5 text-xs rounded bg-red-50 text-red-600 hover:bg-red-100 font-medium"
            >
              <ArrowRightOnRectangleIcon className="w-4 h-4" />
              <span>Đăng xuất</span>
            </button>
          </div>
        </header>

        <main className="flex-1 overflow-auto p-6">{children}</main>
      </div>
    </div>
  );
};
