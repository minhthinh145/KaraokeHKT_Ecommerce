import React, { useState } from "react";
import { EyeIcon, EyeSlashIcon } from "@heroicons/react/24/outline";
import { ChangePasswordOtpModal } from "../../Auth/OtpVerification/ChangePasswordOtpModal";
import { useChangePassword } from "../../../hooks/useChangePassword ";

export const ChangePasswordSection: React.FC = () => {
  const [showPasswords, setShowPasswords] = useState({
    current: false,
    new: false,
    confirm: false,
  });

  // 🔥 Sử dụng useChangePassword hook
  const {
    // States
    currentStep,
    formData,
    loading,

    // OTP related
    otpCode,
    isVerifying,
    isSendingOtp,
    resendButtonText,
    canResend,

    // Actions
    handleInputChange,
    handleRequestChangePassword,
    handleConfirmChangePassword,
    handleOtpInputChange,
    handleReset,
    handleCancel,

    // Computed
    isFormValid,
    canSubmit,
  } = useChangePassword();

  const togglePasswordVisibility = (field: keyof typeof showPasswords) => {
    setShowPasswords((prev) => ({
      ...prev,
      [field]: !prev[field],
    }));
  };

  // 🔥 Success Step
  if (currentStep === "success") {
    return (
      <div className="p-6">
        <div className="max-w-2xl mx-auto text-center py-12">
          <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <span className="text-2xl">✅</span>
          </div>
          <h3 className="text-lg font-semibold text-gray-800 mb-2">
            Đổi mật khẩu thành công!
          </h3>
          <p className="text-gray-600 mb-6">
            Mật khẩu của bạn đã được cập nhật thành công.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className="max-w-2xl mx-auto space-y-6">
        {/* Step 1: Password Input Form */}
        {currentStep === "input" && (
          <>
            {/* Action Button */}
            <div className="flex justify-center mb-8">
              <button
                onClick={handleRequestChangePassword}
                disabled={!canSubmit}
                className={`px-6 py-3 rounded-lg font-semibold transition-all duration-200 ${
                  !canSubmit
                    ? "bg-gray-400 text-white cursor-not-allowed"
                    : "bg-red-500 hover:bg-red-600 text-white"
                }`}
              >
                {loading ? "Đang gửi OTP..." : "Đổi mật khẩu"}
              </button>
            </div>

            {/* Password Fields */}
            <div className="space-y-6">
              {/* Current Password */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Mật khẩu hiện tại *
                </label>
                <div className="relative">
                  <input
                    type={showPasswords.current ? "text" : "password"}
                    value={formData.currentPassword}
                    onChange={(e) =>
                      handleInputChange("currentPassword", e.target.value)
                    }
                    placeholder="Nhập mật khẩu hiện tại"
                    className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-red-500"
                  />
                  <button
                    type="button"
                    onClick={() => togglePasswordVisibility("current")}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                  >
                    {showPasswords.current ? (
                      <EyeSlashIcon className="w-5 h-5" />
                    ) : (
                      <EyeIcon className="w-5 h-5" />
                    )}
                  </button>
                </div>
              </div>

              {/* New Password */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Mật khẩu mới *
                </label>
                <div className="relative">
                  <input
                    type={showPasswords.new ? "text" : "password"}
                    value={formData.newPassword}
                    onChange={(e) =>
                      handleInputChange("newPassword", e.target.value)
                    }
                    placeholder="Nhập mật khẩu mới (ít nhất 6 ký tự)"
                    className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-red-500"
                  />
                  <button
                    type="button"
                    onClick={() => togglePasswordVisibility("new")}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                  >
                    {showPasswords.new ? (
                      <EyeSlashIcon className="w-5 h-5" />
                    ) : (
                      <EyeIcon className="w-5 h-5" />
                    )}
                  </button>
                </div>
                {/* Password strength indicator */}
                {formData.newPassword && (
                  <div className="mt-2">
                    <div className="flex space-x-1">
                      {[...Array(3)].map((_, i) => (
                        <div
                          key={i}
                          className={`h-1 flex-1 rounded ${
                            formData.newPassword.length > i * 3 + 3
                              ? formData.newPassword.length > 8
                                ? "bg-green-500"
                                : formData.newPassword.length > 6
                                ? "bg-yellow-500"
                                : "bg-red-500"
                              : "bg-gray-200"
                          }`}
                        />
                      ))}
                    </div>
                    <p className="text-xs text-gray-500 mt-1">
                      Độ mạnh:{" "}
                      {formData.newPassword.length > 8
                        ? "Mạnh"
                        : formData.newPassword.length > 6
                        ? "Trung bình"
                        : "Yếu"}
                    </p>
                  </div>
                )}
              </div>

              {/* Confirm Password */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Xác nhận mật khẩu mới *
                </label>
                <div className="relative">
                  <input
                    type={showPasswords.confirm ? "text" : "password"}
                    value={formData.confirmPassword}
                    onChange={(e) =>
                      handleInputChange("confirmPassword", e.target.value)
                    }
                    placeholder="Nhập lại mật khẩu mới"
                    className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-red-500 focus:border-red-500"
                  />
                  <button
                    type="button"
                    onClick={() => togglePasswordVisibility("confirm")}
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
                  >
                    {showPasswords.confirm ? (
                      <EyeSlashIcon className="w-5 h-5" />
                    ) : (
                      <EyeIcon className="w-5 h-5" />
                    )}
                  </button>
                </div>
                {/* Password match indicator */}
                {formData.confirmPassword && (
                  <p
                    className={`text-xs mt-1 ${
                      formData.newPassword === formData.confirmPassword
                        ? "text-green-600"
                        : "text-red-600"
                    }`}
                  >
                    {formData.newPassword === formData.confirmPassword
                      ? "✓ Mật khẩu khớp"
                      : "✗ Mật khẩu không khớp"}
                  </p>
                )}
              </div>

              {/* Validation Messages */}
              {!isFormValid &&
                (formData.currentPassword ||
                  formData.newPassword ||
                  formData.confirmPassword) && (
                  <div className="bg-red-50 border border-red-200 rounded-lg p-4">
                    <h4 className="font-medium text-red-800 mb-2">⚠️ Lưu ý:</h4>
                    <ul className="text-sm text-red-700 space-y-1">
                      {!formData.currentPassword && (
                        <li>• Vui lòng nhập mật khẩu hiện tại</li>
                      )}
                      {formData.newPassword.length < 6 && (
                        <li>• Mật khẩu mới phải có ít nhất 6 ký tự</li>
                      )}
                      {formData.newPassword !== formData.confirmPassword && (
                        <li>• Xác nhận mật khẩu không khớp</li>
                      )}
                      {formData.currentPassword === formData.newPassword &&
                        formData.currentPassword && (
                          <li>• Mật khẩu mới phải khác mật khẩu hiện tại</li>
                        )}
                    </ul>
                  </div>
                )}

              {/* Security Tips */}
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
                <h4 className="font-medium text-blue-800 mb-2">
                  💡 Lời khuyên bảo mật:
                </h4>
                <ul className="text-sm text-blue-700 space-y-1">
                  <li>• Sử dụng ít nhất 6 ký tự</li>
                  <li>• Kết hợp chữ hoa, chữ thường và số</li>
                  <li>• Không sử dụng thông tin cá nhân</li>
                  <li>• Mật khẩu mới phải khác mật khẩu cũ</li>
                </ul>
              </div>
            </div>
          </>
        )}

        {/* Step 2: OTP Verification Modal */}
        {currentStep === "otp" && (
          <ChangePasswordOtpModal
            isOpen={true}
            onClose={handleCancel}
            otpCode={otpCode}
            onOtpChange={handleOtpInputChange}
            onVerify={handleConfirmChangePassword}
            isVerifying={isVerifying}
          />
        )}
      </div>
    </div>
  );
};
