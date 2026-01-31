import { useState, useEffect, useCallback } from "react";
import { SendOtp, VerifyAccount } from "../api/services/validateAccount";
import type { VerifyAccountDTO } from "../api/types/auth/VerifyAccountDTO";
import { useToast } from "./useToast";

export interface UseOtpVerificationResult {
  otpCode: string;
  setOtpCode: (v: string) => void;
  isVerifying: boolean;
  isSendingOtp: boolean;
  resendButtonText: string;
  canResend: boolean;
  handleOtpInputChange: (v: string) => void;
  handleSendOtp: (email: string) => Promise<void>;
  handleVerifyOtp: (email: string, onSuccess: () => void) => Promise<void>;
  resetOtpState: () => void;
}

export const useOtpVerification = (): UseOtpVerificationResult => {
  const { showSuccess, showError } = useToast();
  const [otpCode, setOtpCode] = useState("");
  const [isVerifying, setIsVerifying] = useState(false);
  const [isSendingOtp, setIsSendingOtp] = useState(false);
  const [countdown, setCountdown] = useState(0);
  const [canResend, setCanResend] = useState(true);

  // Countdown timer
  useEffect(() => {
    let timer: ReturnType<typeof setTimeout>;
    if (countdown > 0) {
      timer = setTimeout(() => setCountdown(countdown - 1), 1000);
    } else if (countdown === 0 && !canResend) {
      setCanResend(true);
    }
    return () => clearTimeout(timer);
  }, [countdown, canResend]);

  const handleSendOtp = useCallback(
    async (email: string) => {
      if (!canResend) return;
      setIsSendingOtp(true);
      try {
        const response = await SendOtp(email);
        if (response.isSuccess) {
          showSuccess(
            response.message || "Mã OTP đã được gửi đến email của bạn"
          );
          setCountdown(60);
          setCanResend(false);
        } else {
          showError(response.message || "Gửi OTP thất bại");
        }
      } catch (error: any) {
        showError(error.message || "Có lỗi xảy ra khi gửi OTP");
      } finally {
        setIsSendingOtp(false);
      }
    },
    [canResend, showError, showSuccess]
  );

  const handleVerifyOtp = useCallback(
    async (email: string, onSuccess: () => void) => {
      if (!otpCode || otpCode.length !== 6) {
        showError("Mã OTP phải có đúng 6 số");
        return;
      }
      const verifyData: VerifyAccountDTO = {
        Email: email,
        OtpCode: otpCode,
      };
      setIsVerifying(true);
      try {
        const response = await VerifyAccount(verifyData);
        if (response.isSuccess) {
          showSuccess(response.message || "Xác thực tài khoản thành công!");
          onSuccess();
        } else {
          showError(response.message || "Mã OTP không đúng");
        }
      } catch (error: any) {
        showError(error.message || "Có lỗi xảy ra khi xác thực");
      } finally {
        setIsVerifying(false);
      }
    },
    [otpCode, showError, showSuccess]
  );

  const resetOtpState = () => {
    setOtpCode("");
    setCountdown(0);
    setCanResend(true);
    setIsVerifying(false);
    setIsSendingOtp(false);
  };

  const handleOtpInputChange = (value: string) => {
    const numericValue = value.replace(/\D/g, "");
    if (numericValue.length <= 6) setOtpCode(numericValue);
  };

  return {
    otpCode,
    setOtpCode,
    isVerifying,
    isSendingOtp,
    resendButtonText: isSendingOtp
      ? "Đang gửi..."
      : canResend
      ? "Gửi lại mã"
      : `Gửi lại sau ${countdown}s`,
    canResend,
    handleOtpInputChange,
    handleSendOtp,
    handleVerifyOtp,
    resetOtpState,
  };
};
