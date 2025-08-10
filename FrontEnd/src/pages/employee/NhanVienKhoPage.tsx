import React from "react";
import { AdminLayout } from "../../components/admins/uiForAll/AdminLayout";

export const NhanVienKhoPage: React.FC = () => {
  return (
    <div className="flex items-center justify-center min-h-[400px]">
      <div className="text-center space-y-4">
        <div className="text-6xl">📦👷‍♂️</div>
        <h2 className="text-2xl font-bold text-gray-700">Nhân viên kho</h2>
        <p className="text-gray-500 max-w-md">
          Chức năng quản lý kho dành cho nhân viên sẽ sớm có. Bạn sẽ có thể nhập
          xuất hàng, kiểm kê tồn kho tại đây.
        </p>
        <div className="inline-flex items-center px-4 py-2 bg-green-50 rounded-lg">
          <span className="text-green-600 font-medium">
            🚧 Đang phát triển...
          </span>
        </div>
      </div>
    </div>
  );
};
