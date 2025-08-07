import React, { useState, useEffect } from "react";
import { useQLNhanSu } from "../../../../hooks/useQLNhanSu";
import type { NhanVienDTO } from "../../../../api";
import { formatDateForDisplay } from "../../../../api/services/admin/utils/dateUtils";
import { formatDateForInput } from "../../../../api/services/admin/utils/dateUtils";

interface EditNhanVienModalProps {
  isOpen: boolean;
  onClose: () => void;
  nhanVien: NhanVienDTO | null | undefined;
  roleOptions: Array<{ value: string; label: string }>;
}

export const EditNhanVienModal: React.FC<EditNhanVienModalProps> = ({
  isOpen,
  onClose,
  nhanVien,
  roleOptions,
}) => {
  const { loading, handlers, actions } = useQLNhanSu();

  const [formData, setFormData] = useState<NhanVienDTO>({
    maNv: "",
    hoTen: "",
    email: "",
    ngaySinh: "",
    soDienThoai: "",
    loaiNhanVien: "",
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // 🔄 Update form when nhanVien changes
  useEffect(() => {
    if (nhanVien) {
      setFormData({
        maNv: nhanVien.maNv,
        hoTen: nhanVien.hoTen,
        email: nhanVien.email,
        ngaySinh: formatDateForInput(nhanVien.ngaySinh),
        soDienThoai: nhanVien.soDienThoai || "",
        loaiNhanVien: nhanVien.loaiNhanVien || "",
      });
      setError(null); // Reset error when opening new edit
    }
  }, [nhanVien]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError(null);

    try {
      const result = await handlers.updateNhanVien(formData);

      if (result.success) {
        actions.closeEditModal();
        // Optional: Show success message
      } else {
        setError(result.error || "Có lỗi xảy ra khi cập nhật nhân viên");
      }
    } catch (error: any) {
      console.error("Error updating nhân viên:", error);
      setError(error.message || "Có lỗi xảy ra khi cập nhật nhân viên");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleChange = (field: keyof NhanVienDTO, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    if (error) setError(null);
  };

  const handleClose = () => {
    actions.closeEditModal();
    setError(null); // Reset error on close
  };

  // 🔥 Don't render if modal not open or no data
  if (!isOpen || !nhanVien) return null;

  return (
    <>
      {/* 🔥 Fixed Backdrop - separate element */}
      <div
        className="fixed inset-0 z-[9998] bg-black bg-opacity-50 transition-opacity"
        onClick={handleClose}
        aria-hidden="true"
      />

      {/* 🔥 Modal Container */}
      <div className="fixed inset-0 z-[9999] overflow-y-auto">
        <div className="flex items-center justify-center min-h-screen pt-4 px-4 pb-20">
          {/* 🔥 Modal Content - WHITE background */}
          <div className="relative bg-white rounded-xl shadow-2xl transform transition-all w-full max-w-lg">
            <form onSubmit={handleSubmit}>
              {/* 🔥 Header */}
              <div className="bg-white px-6 pt-6 pb-4 rounded-t-xl">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-xl font-semibold text-gray-900">
                    Chỉnh sửa thông tin nhân viên
                  </h3>
                  <button
                    type="button"
                    onClick={handleClose}
                    className="text-gray-400 hover:text-gray-600 p-2 hover:bg-gray-100 rounded-full transition-colors"
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

                {/* 🔥 Error Display */}
                {error && (
                  <div className="mb-4 p-4 bg-red-50 border border-red-200 text-red-700 rounded-lg text-sm">
                    <div className="flex">
                      <svg
                        className="w-5 h-5 text-red-400 mr-2 flex-shrink-0"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                        />
                      </svg>
                      <span>{error}</span>
                    </div>
                  </div>
                )}
              </div>

              {/* 🔥 Form Content */}
              <div className="bg-white px-6 pb-6">
                <div className="space-y-5">
                  {/* Mã NV - Readonly */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Mã nhân viên
                    </label>
                    <input
                      type="text"
                      value={formData.maNv}
                      disabled
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg bg-gray-50 text-gray-500 cursor-not-allowed"
                    />
                  </div>

                  {/* Họ tên */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Họ tên <span className="text-red-500">*</span>
                    </label>
                    <input
                      type="text"
                      required
                      value={formData.hoTen}
                      onChange={(e) => handleChange("hoTen", e.target.value)}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors"
                      placeholder="Nhập họ tên nhân viên"
                    />
                  </div>

                  {/* Email - Readonly */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Email
                    </label>
                    <input
                      type="email"
                      value={formData.email}
                      disabled
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg bg-gray-50 text-gray-500 cursor-not-allowed"
                    />
                    <p className="text-xs text-gray-500 mt-1">
                      Email không thể thay đổi
                    </p>
                  </div>

                  {/* SĐT */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Số điện thoại
                    </label>
                    <input
                      type="tel"
                      value={formData.soDienThoai}
                      onChange={(e) =>
                        handleChange("soDienThoai", e.target.value)
                      }
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors"
                      placeholder="0123456789"
                    />
                  </div>

                  {/* Ngày sinh */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Ngày sinh <span className="text-red-500">*</span>
                    </label>
                    <input
                      type="date"
                      required
                      value={formData.ngaySinh}
                      onChange={(e) => handleChange("ngaySinh", e.target.value)}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors"
                      max={new Date().toISOString().split("T")[0]}
                    />
                  </div>

                  {/* Loại nhân viên */}
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Loại nhân viên <span className="text-red-500">*</span>
                    </label>
                    <select
                      required
                      value={formData.loaiNhanVien}
                      onChange={(e) =>
                        handleChange("loaiNhanVien", e.target.value)
                      }
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-colors"
                    >
                      <option value="">-- Chọn loại nhân viên --</option>
                      {roleOptions
                        .filter((opt) => opt.value !== "All")
                        .map((option) => (
                          <option key={option.value} value={option.value}>
                            {option.label}
                          </option>
                        ))}
                    </select>
                  </div>
                </div>
              </div>

              {/* 🔥 Footer */}
              <div className="bg-gray-50 px-6 py-4 flex justify-end gap-3 rounded-b-xl">
                <button
                  type="button"
                  onClick={handleClose}
                  disabled={isSubmitting}
                  className="px-6 py-2.5 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 transition-colors"
                >
                  Hủy
                </button>
                <button
                  type="submit"
                  disabled={isSubmitting || loading.nhanVien}
                  className="px-6 py-2.5 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-lg hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                >
                  {isSubmitting || loading.nhanVien ? (
                    <div className="flex items-center">
                      <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                      Đang cập nhật...
                    </div>
                  ) : (
                    "Cập nhật"
                  )}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </>
  );
};
