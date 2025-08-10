import React, { useState } from "react";
import {
  MagnifyingGlassIcon,
  FunnelIcon,
  Bars3Icon,
  XMarkIcon,
  ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/outline";
import { useAuth } from "../../../hooks/auth/useAuth"; // Đảm bảo đúng path

interface AdminLayoutProps {
  children: React.ReactNode;
  sidebarContent: React.ReactNode;
  pageTitle?: string;
  searchPlaceholder?: string;
  showSearch?: boolean;
  showFilter?: boolean;
}

export const AdminLayout: React.FC<AdminLayoutProps> = ({
  children,
  sidebarContent,
  pageTitle = "Quản lý hệ thống",
  searchPlaceholder = "Tìm kiếm...",
  showSearch = true,
  showFilter = true,
}) => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const { logout } = useAuth();

  return (
    <div className="h-screen w-screen bg-white shadow-[0px_4px_4px_0px_rgba(0,0,0,0.25)] border border-black flex overflow-hidden">
      {/* 🔥 Sidebar - Fixed width */}
      <div
        className={`${
          isSidebarOpen ? "w-80" : "w-20"
        } transition-all duration-300 bg-white border-r border-neutral-200 flex flex-col relative flex-shrink-0`}
      >
        {/* Sidebar Header */}
        <div className="p-6 border-b border-neutral-200">
          <div className="flex items-center justify-between">
            {isSidebarOpen && (
              <h1 className="text-xl font-bold font-['Space_Grotesk'] text-black truncate">
                {pageTitle}
              </h1>
            )}
            <button
              onClick={() => setIsSidebarOpen(!isSidebarOpen)}
              className="p-1.5 rounded-lg hover:bg-gray-100 transition-colors flex-shrink-0"
            >
              {isSidebarOpen ? (
                <XMarkIcon className="w-5 h-5" />
              ) : (
                <Bars3Icon className="w-5 h-5" />
              )}
            </button>
          </div>
        </div>

        {/* 🔥 Sidebar Content */}
        <div className="flex-1 overflow-y-auto overflow-x-hidden">
          {sidebarContent}
        </div>
      </div>

      {/* 🔥 Main Content Area - CHÍNH LÀ PHẦN NÀY! */}
      <div className="flex-1 flex flex-col min-w-0">
        {/* Top Header Bar */}
        <header className="bg-white border-b border-neutral-200 px-6 py-4 flex-shrink-0">
          <div
            className={`flex items-center gap-8 ${
              !showSearch && !showFilter ? "justify-end" : "justify-between"
            }`}
          >
            {/* Search & Filter Section */}
            {(showSearch || showFilter) && (
              <div className="flex items-center gap-4 min-w-0">
                {/* Search Input */}
                {showSearch && (
                  <div className="relative">
                    <MagnifyingGlassIcon className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-zinc-500" />
                    <input
                      type="text"
                      placeholder={searchPlaceholder}
                      value={searchQuery}
                      onChange={(e) => setSearchQuery(e.target.value)}
                      className="w-96 max-w-full pl-10 pr-4 py-2 bg-white rounded-lg border border-neutral-200 focus:ring-2 focus:ring-indigo-500 focus:border-transparent text-base text-zinc-500 placeholder-zinc-500"
                    />
                  </div>
                )}

                {/* Filter Button */}
                {showFilter && (
                  <button className="flex items-center gap-3 px-4 py-2 bg-white rounded-lg border border-neutral-200 hover:bg-gray-50 transition-colors whitespace-nowrap">
                    <FunnelIcon className="w-5 h-5 text-zinc-500" />
                    <span className="text-xl font-bold font-['Space_Grotesk'] text-zinc-500">
                      Filter
                    </span>
                  </button>
                )}
              </div>
            )}

            {/* User Info Section */}
            <div className="flex items-center gap-4 flex-shrink-0">
              <div className="w-8 h-8 bg-indigo-500 rounded-full flex items-center justify-center">
                <span className="text-white text-sm font-bold">A</span>
              </div>
              <span className="font-medium text-gray-700 whitespace-nowrap">
                Tài khoản quản lý
              </span>
              <button
                onClick={logout}
                className="flex items-center gap-1 px-3 py-1.5 rounded-lg bg-red-50 hover:bg-red-100 text-red-600 font-semibold transition"
                title="Đăng xuất"
              >
                <ArrowRightOnRectangleIcon className="w-5 h-5" />
                <span className="hidden sm:inline">Đăng xuất</span>
              </button>
            </div>
          </div>
        </header>

        {/* 🔥 Page Content - ĐÂY LÀ PHẦN QUAN TRỌNG NHẤT! */}
        <main className="flex-1 bg-white p-6 overflow-auto min-w-0">
          <div className="w-full min-w-0">{children}</div>
        </main>
      </div>
    </div>
  );
};
