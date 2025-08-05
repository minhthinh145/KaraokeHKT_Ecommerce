import React, { useEffect } from "react";
import { Input } from "../ui/Input";
import { Button } from "../ui/Button";
import { useOtpVerification } from "../../hooks/useOtpVerification";

interface OtpVerificationModalProps {
  isOpen: boolean;
  onClose: () => void;
  onVerificationSuccess: () => void;
  userEmail: string;
}

export const OtpVerificationModal: React.FC<OtpVerificationModalProps> = ({
  isOpen,
  onClose,
  onVerificationSuccess,
  userEmail,
}) => {
  const {
    otpCode,
    isVerifying,
    resendButtonText,
    handleOtpInputChange,
    handleSendOtp,
    handleVerifyOtp,
    resetOtpState,
  } = useOtpVerification();

  // Auto send OTP when modal opens
  useEffect(() => {
    if (isOpen) {
      handleSendOtp(userEmail);
    }
    // eslint-disable-next-line
  }, [isOpen, userEmail]);

  const handleClose = () => {
    resetOtpState();
    onClose();
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    handleOtpInputChange(e.target.value);
  };

  const handleVerify = () => {
    handleVerifyOtp(userEmail, () => {
      onVerificationSuccess();
      handleClose();
    });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-full max-w-md mx-4">
        <div className="text-center mb-6">
          <h2 className="text-2xl font-bold text-gray-900 mb-2">
            Xác thực tài khoản
          </h2>
          <p className="text-gray-600">
            Chúng tôi đã gửi mã OTP 6 số đến email:
          </p>
          <p className="text-blue-600 font-medium mt-1">{userEmail}</p>
        </div>

        <div className="space-y-4">
          <div>
            <Input
              label="Mã OTP"
              type="text"
              placeholder="Nhập mã OTP 6 số"
              value={otpCode}
              onChange={handleInputChange}
              required
            />
            <p className="text-sm text-gray-500 mt-1">
              Vui lòng kiểm tra hộp thư và thư mục spam
            </p>
          </div>

          <Button type="button" onClick={handleVerify} fullWidth>
            {isVerifying ? "Đang xác thực..." : "Xác thực OTP"}
          </Button>

          <div className="flex items-center justify-between">
            <Button type="button" onClick={() => handleSendOtp(userEmail)}>
              {resendButtonText}
            </Button>

            <button
              type="button"
              onClick={handleClose}
              className="text-gray-600 hover:text-gray-800 text-sm"
            >
              Đóng
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
