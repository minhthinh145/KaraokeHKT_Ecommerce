import { useCallback, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "./auth/useAuth";
import { useToast } from "./useToast";
import type { SignInDTO } from "../api/types/auth/AuthDTO";

export const useSignInForm = () => {
  const {
    login,
    loading,
    error,
    clearError: clearAuthError,
    navigateToDefaultRoute,
  } = useAuth();
  const navigate = useNavigate();
  const { showSuccess, showError } = useToast();

  // 🔥 State cho activation modal (chỉ dành cho form)
  const [showActivationModal, setShowActivationModal] = useState(false);
  const [pendingEmail, setPendingEmail] = useState("");

  const handleLogin = useCallback(
    async (credentials: SignInDTO) => {
      try {
        if (error) {
          clearAuthError();
        }

        const result = await login(credentials);

        // 🎉 Login thành công
        showSuccess("Đăng nhập thành công!");

        return { success: true, data: result };
      } catch (rejectedValue: any) {
        let errorMsg = "Đăng nhập thất bại";
        let needActivation = false;

        // 🔥 Kiểm tra lỗi activation
        if (typeof rejectedValue === "string") {
          errorMsg = rejectedValue;
        } else if (rejectedValue?.message) {
          errorMsg = rejectedValue.message;
          // 🎯 CHECK: data === false (account not active)
          if (rejectedValue?.data === false) {
            needActivation = true;
          }
        } else if (rejectedValue?.response?.data) {
          const responseData = rejectedValue.response.data;
          errorMsg = responseData.message || errorMsg;
          // 🎯 CHECK: data === false trong response
          if (responseData.data === false) {
            needActivation = true;
          }
        }

        // 🔥 Nếu cần activation, show modal
        if (needActivation) {
          setPendingEmail(credentials.email);
          setShowActivationModal(true);
          return { success: false, error: errorMsg, needActivation: true };
        }

        // 🔥 Các lỗi khác show toast
        showError("Email hoặc mật khẩu không đúng");
        return { success: false, error: errorMsg };
      }
    },
    [login, error, clearAuthError, showSuccess, showError, navigate]
  );

  // 🔥 Handle activation modal actions
  const handleActivationConfirm = useCallback(() => {
    setShowActivationModal(false);
    return pendingEmail;
  }, [pendingEmail]);

  const handleActivationCancel = useCallback(() => {
    setShowActivationModal(false);
    setPendingEmail("");
  }, []);

  return {
    loading,
    error,
    login: handleLogin,
    clearAuthError,
    showActivationModal,
    pendingEmail,
    handleActivationConfirm,
    handleActivationCancel,
  };
};
