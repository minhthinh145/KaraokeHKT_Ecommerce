import { useState } from "react";
import { useOtpVerification } from "./useOtpVerification";
import {
  requestChangePassword,
  confirmChangePassword,
} from "../api/services/changePassword";
import type {
  ChangePasswordDTO,
  ConfirmChangePasswordDTO,
} from "../api/types/auth/ChangePassword";
import { useToast } from "./useToast";

type ChangePasswordStep = "input" | "otp" | "success";

interface PasswordFormData {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

export const useChangePassword = () => {
  const { showSuccess, showError } = useToast();
  const [currentStep, setCurrentStep] = useState<ChangePasswordStep>("input");
  const [loading, setLoading] = useState(false);

  const [formData, setFormData] = useState<PasswordFormData>({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  // Sử dụng lại useOtpVerification hook
  const {
    otpCode,
    isVerifying,
    isSendingOtp,
    resendButtonText,
    canResend,
    handleOtpInputChange,
    resetOtpState,
  } = useOtpVerification();

  // Validate form data
  const isFormValid = () => {
    return (
      formData.currentPassword.trim() !== "" &&
      formData.newPassword.length >= 6 &&
      formData.newPassword === formData.confirmPassword &&
      formData.currentPassword !== formData.newPassword
    );
  };

  // Handle input changes
  const handleInputChange = (field: keyof PasswordFormData, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  // Step 1: Request change password (send OTP)
  const handleRequestChangePassword = async () => {
    if (!isFormValid()) {
      showError("Vui lòng kiểm tra lại thông tin đã nhập");
      return;
    }

    setLoading(true);
    try {
      const changePasswordData: ChangePasswordDTO = {
        oldPassword: formData.currentPassword,
        newPassword: formData.newPassword,
        confirmPassword: formData.confirmPassword,
      };

      const response = await requestChangePassword(changePasswordData);

      if (response.isSuccess) {
        showSuccess(response.message || "Mã OTP đã được gửi đến email của bạn");
        setCurrentStep("otp");
      } else {
        showError(response.message || "Yêu cầu đổi mật khẩu thất bại");
      }
    } catch (error: any) {
      showError(error.message || "Có lỗi xảy ra khi yêu cầu đổi mật khẩu");
    } finally {
      setLoading(false);
    }
  };

  // Step 2: Confirm change password with OTP
  const handleConfirmChangePassword = async () => {
    if (!otpCode || otpCode.length !== 6) {
      showError("Mã OTP phải có đúng 6 số");
      return;
    }

    try {
      const confirmData: ConfirmChangePasswordDTO = {
        newPassword: formData.newPassword,
        otpCode: otpCode,
      };

      const response = await confirmChangePassword(confirmData);

      if (response.isSuccess) {
        showSuccess(response.message || "Đổi mật khẩu thành công!");
        setCurrentStep("success");

        // Auto reset after 2 seconds
        setTimeout(() => {
          handleReset();
        }, 2000);
      } else {
        showError(response.message || "Xác thực OTP thất bại");
      }
    } catch (error: any) {
      showError(error.message || "Có lỗi xảy ra khi xác nhận đổi mật khẩu");
    }
  };

  // Reset all states
  const handleReset = () => {
    setCurrentStep("input");
    setFormData({
      currentPassword: "",
      newPassword: "",
      confirmPassword: "",
    });
    resetOtpState();
    setLoading(false);
  };

  // Cancel change password process
  const handleCancel = () => {
    if (currentStep === "otp") {
      setCurrentStep("input");
      resetOtpState();
    } else {
      handleReset();
    }
  };

  return {
    // States
    currentStep,
    formData,
    loading,

    // OTP related (from useOtpVerification)
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
    isFormValid: isFormValid(),
    canSubmit: isFormValid() && !loading,
  };
};
