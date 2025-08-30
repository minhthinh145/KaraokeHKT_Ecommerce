import React, { useEffect } from "react";
import { OtpVerificationModal } from "../OtpVerificationModal";
import { useOtpVerification } from "../../../hooks/useOtpVerification";

interface LoginOtpModalProps {
  isOpen: boolean;
  onClose: () => void;
  onVerificationSuccess: () => void;
  userEmail: string;
  autoSend: boolean;
}

export const LoginOtpModal: React.FC<LoginOtpModalProps> = ({
  isOpen,
  onClose,
  onVerificationSuccess,
  userEmail,
  autoSend = false,
}) => {
  const {
    otpCode,
    isVerifying,
    isSendingOtp,
    resendButtonText,
    canResend,
    handleOtpInputChange,
    handleSendOtp,
    handleVerifyOtp,
    resetOtpState,
  } = useOtpVerification();

  // Auto send OTP when modal opens
  useEffect(() => {
    if (autoSend && isOpen) {
      handleSendOtp(userEmail);
    }
  }, [isOpen, userEmail, handleSendOtp]);

  const handleClose = () => {
    resetOtpState();
    onClose();
  };

  const handleVerify = () => {
    handleVerifyOtp(userEmail, () => {
      onVerificationSuccess();
      handleClose();
    });
  };

  const handleResend = () => {
    handleSendOtp(userEmail);
  };

  return (
    <OtpVerificationModal
      isOpen={isOpen}
      onClose={handleClose}
      title="Xác thực tài khoản"
      subtitle="Chúng tôi đã gửi mã OTP 6 số đến email:"
      userEmail={userEmail}
      otpCode={otpCode}
      onOtpChange={handleOtpInputChange}
      onVerify={handleVerify}
      onResend={handleResend}
      isVerifying={isVerifying}
      isSendingOtp={isSendingOtp}
      resendButtonText={resendButtonText}
      canResend={canResend}
      showResendButton={true}
      verifyButtonText="Xác thực tài khoản"
    />
  );
};
