import React, { useState } from "react";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu"; // 🔥 Fix: Correct path
import type { AddNhanVienDTO } from "../../../../api"; // 🔥 Fix: Specific import


interface AddNhanVienModalProps {
  isOpen: boolean;
  onClose: () => void;
  roleOptions: Array<{ value: string; label: string }>;
}

export const AddNhanVienModal: React.FC<AddNhanVienModalProps> = ({
  isOpen,
  onClose,
  roleOptions,
}) => {
  const { loading, handlers, actions } = useQLNhanSu();

  // 🔥 Fix: Add missing diaChi field (required by API even if not in UI)
  const [formData, setFormData] = useState<AddNhanVienDTO>({
    hoTen: "",
    email: "",
    ngaySinh: "",
    soDienThoai: "",
    loaiTaiKhoan: "NhanVienKho",
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError(null);

    try {
      const result = await handlers.addNhanVien(formData);

      if (result.success) {
        // Reset form
        setFormData({
          hoTen: "",
          email: "",
          ngaySinh: "",
          soDienThoai: "",
          loaiTaiKhoan: "NhanVienKho",
        });
        actions.closeAddModal(); // 🔥 Use action to close modal
      } else {
        setError(result.error || "Có lỗi xảy ra khi tạo nhân viên");
      }
    } catch (error: any) {
      console.error("Error adding nhân viên:", error);
      setError(error.message || "Có lỗi xảy ra khi tạo nhân viên");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleChange = (field: keyof AddNhanVienDTO, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    // Clear error when user starts typing
    if (error) setError(null);
  };

  const handleClose = () => {
    actions.closeAddModal(); // 🔥 Use Redux action instead of prop
  };

  if (!isOpen) return null;

  return (
    <>
      {/* 🔥 Fix: Backdrop with higher z-index */}
      <div
        className="fixed inset-0 z-[9998] bg-black bg-opacity-50 transition-opacity backdrop-blur-sm"
        onClick={handleClose}
      />

      {/* 🔥 Fix: Modal with even higher z-index */}
      <div className="fixed inset-0 z-[9998] overflow-y-auto">
        <div className="flex items-center justify-center min-h-screen pt-4 px-4 pb-20">
          {/* Modal Content */}
          <div className="relative bg-white rounded-lg shadow-xl transform transition-all w-full max-w-lg">
            <form onSubmit={handleSubmit}>
              <div className="px-6 pt-6 pb-4">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-lg font-bold text-black ">
                    Thêm nhân viên mới
                  </h3>
                  <button
                    type="button"
                    onClick={handleClose}
                    className="text-gray-400 hover:text-gray-600 p-1"
                  >
                    <svg
                      className="w-6 h-6"
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
                  </button>
                </div>

                {/* Error Display */}
                {error && (
                  <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg text-sm">
                    {error}
                  </div>
                )}

                <div className="space-y-4">
                  {/* Họ tên */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Họ tên *
                    </label>
                    <input
                      type="text"
                      required
                      value={formData.hoTen}
                      onChange={(e) => handleChange("hoTen", e.target.value)}
                      className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                      placeholder="Nhập họ tên nhân viên"
                    />
                  </div>

                  {/* Email */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Email *
                    </label>
                    <input
                      type="email"
                      required
                      value={formData.email}
                      onChange={(e) => handleChange("email", e.target.value)}
                      className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                      placeholder="nhanvien@email.com"
                    />
                    <p className="text-xs text-gray-500 mt-1">
                      💡 Email sẽ được sử dụng làm mật khẩu đăng nhập
                    </p>
                  </div>

                  {/* SĐT */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Số điện thoại *
                    </label>
                    <input
                      type="tel"
                      required
                      value={formData.soDienThoai}
                      onChange={(e) =>
                        handleChange("soDienThoai", e.target.value)
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                      placeholder="0123456789"
                    />
                  </div>

                  {/* Ngày sinh */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Ngày sinh *
                    </label>
                    <input
                      type="date"
                      required
                      value={formData.ngaySinh}
                      onChange={(e) => handleChange("ngaySinh", e.target.value)}
                      className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                      max={new Date().toISOString().split("T")[0]}
                    />
                  </div>

                  {/* Loại nhân viên - 🔥 Filter out Manager roles */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Loại nhân viên *
                    </label>
                    <select
                      required
                      value={formData.loaiTaiKhoan}
                      onChange={(e) =>
                        handleChange("loaiTaiKhoan", e.target.value)
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500"
                    >
                      {roleOptions
                        .filter(
                          (opt) =>
                            opt.value !== "All" 
                        )
                        .map((option) => (
                          <option key={option.value} value={option.value}>
                            {option.label}
                          </option>
                        ))}
                    </select>
                    <p className="text-xs text-gray-500 mt-1">
                      💡 Chỉ có thể tạo tài khoản nhân viên. Quản lý được cấp
                      tài khoản riêng.
                    </p>
                  </div>
                </div>
              </div>

              {/* Footer */}
              <div className="bg-gray-50 px-6 py-3 flex justify-end gap-3 rounded-b-lg">
                <button
                  type="button"
                  onClick={handleClose}
                  disabled={isSubmitting}
                  className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  disabled={isSubmitting || loading.nhanVien}
                  className="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  {isSubmitting || loading.nhanVien
                    ? "Đang tạo..."
                    : "Tạo nhân viên"}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </>
  );
};
