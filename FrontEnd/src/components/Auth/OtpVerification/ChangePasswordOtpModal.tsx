import React from "react";
import { OtpVerificationModal } from "../OtpVerificationModal";

interface ChangePasswordOtpModalProps {
  isOpen: boolean;
  onClose: () => void;
  otpCode: string;
  onOtpChange: (value: string) => void;
  onVerify: () => void;
  isVerifying?: boolean;
  userEmail?: string;
}

export const ChangePasswordOtpModal: React.FC<ChangePasswordOtpModalProps> = ({
  isOpen,
  onClose,
  otpCode,
  onOtpChange,
  onVerify,
  isVerifying = false,
  userEmail,
}) => {
  return (
    <OtpVerificationModal
      isOpen={isOpen}
      onClose={onClose}
      title="Xác thực đổi mật khẩu"
      subtitle="Vui lòng nhập mã OTP được gửi đến email của bạn để xác nhận đổi mật khẩu"
      userEmail={userEmail}
      otpCode={otpCode}
      onOtpChange={onOtpChange}
      onVerify={onVerify}
      isVerifying={isVerifying}
      showResendButton={false} // No resend for change password
      verifyButtonText="Xác nhận đổi mật khẩu"
      customMessage="⚠️ Sau khi xác nhận, mật khẩu cũ sẽ không thể sử dụng được nữa"
    />
  );
};
