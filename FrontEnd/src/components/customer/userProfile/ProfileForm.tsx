import React from "react";

interface FormData {
  fullName: string;
  phoneNumber: string;
  email: string;
  dateOfBirth: string;
}

interface ProfileFormProps {
  formData: FormData;
  isEditing: boolean;
  onInputChange: (field: string, value: string) => void;
}

export const ProfileForm: React.FC<ProfileFormProps> = ({
  formData,
  isEditing,
  onInputChange,
}) => {
  return (
    <div className="p-6">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Thông tin cá nhân
      </h2>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Họ và tên */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Họ và tên
          </label>
          {isEditing ? (
            <input
              type="text"
              value={formData.fullName}
              onChange={(e) => onInputChange("fullName", e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
              placeholder="Nhập họ và tên"
            />
          ) : (
            <div className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-lg text-gray-900">
              {formData.fullName || "Chưa cập nhật"}
            </div>
          )}
        </div>

        {/* Số điện thoại */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Số điện thoại
          </label>
          {isEditing ? (
            <input
              type="tel"
              value={formData.phoneNumber}
              onChange={(e) => onInputChange("phoneNumber", e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
              placeholder="Nhập số điện thoại"
            />
          ) : (
            <div className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-lg text-gray-900">
              {formData.phoneNumber || "Chưa cập nhật"}
            </div>
          )}
        </div>

        {/* Email */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Email
          </label>
          <div className="w-full px-4 py-3 bg-gray-100 border border-gray-200 rounded-lg text-gray-500">
            {formData.email || "Chưa cập nhật"}
            <span className="text-xs text-gray-400 ml-2">
              (Không thể thay đổi)
            </span>
          </div>
        </div>

        {/* Ngày sinh */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Ngày sinh
          </label>
          {isEditing ? (
            <input
              type="date"
              value={formData.dateOfBirth}
              onChange={(e) => onInputChange("dateOfBirth", e.target.value)}
              className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all"
            />
          ) : (
            <div className="w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-lg text-gray-900">
              {formData.dateOfBirth
                ? new Date(formData.dateOfBirth).toLocaleDateString("vi-VN")
                : "Chưa cập nhật"}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
