import React from "react";
import { Input } from "../ui/Input";

interface OtpVerificationModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  subtitle?: string;
  userEmail?: string;
  otpCode: string;
  onOtpChange: (value: string) => void;
  onVerify: () => void;
  onResend?: () => void;
  isVerifying?: boolean;
  isSendingOtp?: boolean;
  resendButtonText?: string;
  canResend?: boolean;
  showResendButton?: boolean;
  verifyButtonText?: string;
  customMessage?: string;
}

export const OtpVerificationModal: React.FC<OtpVerificationModalProps> = ({
  isOpen,
  onClose,
  title = "Xác thực OTP",
  subtitle = "Vui lòng nhập mã OTP được gửi đến email của bạn",
  userEmail,
  otpCode,
  onOtpChange,
  onVerify,
  onResend,
  isVerifying = false,
  isSendingOtp = false,
  resendButtonText = "Gửi lại mã",
  canResend = true,
  showResendButton = true,
  verifyButtonText = "Xác thực OTP",
  customMessage,
}) => {
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    // Only allow numbers and limit to 6 digits
    const numericValue = e.target.value.replace(/\D/g, "").slice(0, 6);
    onOtpChange(numericValue);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && otpCode.length === 6 && !isVerifying) {
      onVerify();
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-xl shadow-2xl p-8 w-full max-w-md mx-4">
        {/* Header */}
        <div className="text-center mb-8">
          <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <span className="text-2xl">📧</span>
          </div>
          <h2 className="text-2xl font-bold text-gray-900 mb-3">{title}</h2>
          <p className="text-gray-600 leading-relaxed">{subtitle}</p>
          {userEmail && (
            <p className="text-blue-600 font-semibold mt-2">{userEmail}</p>
          )}
          {customMessage && (
            <div className="mt-4 p-3 bg-orange-50 border border-orange-200 rounded-lg">
              <p className="text-sm text-orange-700">{customMessage}</p>
            </div>
          )}
        </div>

        <div className="space-y-6">
          {/* OTP Input với styling đẹp */}
          <div className="text-center">
            <label className="block text-sm font-semibold text-gray-700 mb-3">
              Mã OTP
              <span className="text-red-500 ml-1">*</span>
            </label>

            {/* Custom OTP Input */}
            <input
              type="text"
              value={otpCode}
              onChange={handleInputChange}
              onKeyPress={handleKeyPress}
              placeholder="000000"
              maxLength={6}
              disabled={isVerifying || isSendingOtp}
              className="w-full text-center text-2xl font-mono font-bold tracking-widest px-4 py-4 border-2 border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 disabled:bg-gray-50 disabled:cursor-not-allowed transition-all duration-200"
            />

            <p className="text-sm text-gray-500 mt-2">
              Vui lòng kiểm tra hộp thư và thư mục spam
            </p>

            {/* OTP Progress Indicator */}
            {otpCode && (
              <div className="mt-4">
                <div className="flex justify-center space-x-2 mb-2">
                  {[...Array(6)].map((_, i) => (
                    <div
                      key={i}
                      className={`w-3 h-3 rounded-full transition-all duration-200 ${
                        i < otpCode.length
                          ? "bg-blue-500 scale-110"
                          : "bg-gray-200"
                      }`}
                    />
                  ))}
                </div>
                <p
                  className={`text-sm font-medium ${
                    otpCode.length === 6 ? "text-green-600" : "text-gray-500"
                  }`}
                >
                  {otpCode.length}/6 ký tự
                  {otpCode.length === 6 && " ✓"}
                </p>
              </div>
            )}
          </div>

          {/* Verify Button - Màu đỏ đẹp */}
          <button
            type="button"
            onClick={onVerify}
            disabled={otpCode.length !== 6 || isVerifying || isSendingOtp}
            className={`w-full py-4 rounded-lg font-semibold text-lg transition-all duration-200 transform ${
              otpCode.length === 6 && !isVerifying && !isSendingOtp
                ? "bg-red-500 hover:bg-red-600 text-white shadow-lg hover:shadow-xl hover:scale-105 active:scale-95"
                : "bg-gray-300 text-gray-500 cursor-not-allowed"
            }`}
          >
            {isVerifying ? (
              <div className="flex items-center justify-center space-x-2">
                <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
                <span>Đang xác thực...</span>
              </div>
            ) : (
              verifyButtonText
            )}
          </button>

          {/* Bottom Actions */}
          <div className="flex items-center justify-center space-x-6 pt-4 border-t border-gray-100">
            {/* Resend Button */}
            {showResendButton && onResend && (
              <button
                type="button"
                onClick={onResend}
                disabled={!canResend || isSendingOtp || isVerifying}
                className="text-blue-600 hover:text-blue-700 font-medium text-sm disabled:text-gray-400 disabled:cursor-not-allowed transition-colors duration-200"
              >
                {isSendingOtp ? (
                  <div className="flex items-center space-x-1">
                    <div className="w-3 h-3 border border-blue-500 border-t-transparent rounded-full animate-spin"></div>
                    <span>Đang gửi...</span>
                  </div>
                ) : (
                  resendButtonText
                )}
              </button>
            )}

            {/* Close Button */}
            <button
              type="button"
              onClick={onClose}
              disabled={isVerifying || isSendingOtp}
              className="text-gray-500 hover:text-gray-700 font-medium text-sm disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200"
            >
              {isVerifying || isSendingOtp ? "Đang xử lý..." : "Hủy"}
            </button>
          </div>

          {/* Additional Info */}
          <div className="text-center pt-2">
            <p className="text-xs text-gray-400">
              🕐 Mã OTP có hiệu lực trong 5 phút
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};
