import { useCallback, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { signInThunk, clearError } from "../redux/auth/index";
import type { RootState, AppDispatch } from "../redux/store";
import type { SignInDTO } from "../api/types/auth/AuthDTO";
import { useToast } from "./useToast";

export const useAuth = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { showSuccess, showError } = useToast();

  // 🔥 State cho activation modal
  const [showActivationModal, setShowActivationModal] = useState(false);
  const [pendingEmail, setPendingEmail] = useState("");

  const { loading, error, isAuthenticated, user } = useSelector(
    (state: RootState) => state.auth
  );

  const login = useCallback(
    async (credentials: SignInDTO) => {
      try {
        if (error) {
          dispatch(clearError());
        }

        const result = await dispatch(signInThunk(credentials)).unwrap();

        showSuccess("Đăng nhập thành công!");

        setTimeout(() => {
          navigate("/", { replace: true });
        }, 500);

        return { success: true, data: result };
      } catch (rejectedValue: any) {
        let errorMsg = "Đăng nhập thất bại";
        let needActivation = false;

        // 🔥 Kiểm tra data === false (account not active)
        if (typeof rejectedValue === "string") {
          errorMsg = rejectedValue;
        } else if (rejectedValue?.message) {
          errorMsg = rejectedValue.message;
          // 🎯 CHECK: data === false
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

        // 🔥 Nếu needActivation = true, show modal
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
    [dispatch, navigate, showSuccess, showError, error]
  );

  const clearAuthError = useCallback(() => {
    dispatch(clearError());
  }, [dispatch]);

  // 🔥 Handle activation modal actions
  const handleActivationConfirm = () => {
    setShowActivationModal(false);
    return pendingEmail;
  };

  const handleActivationCancel = () => {
    setShowActivationModal(false);
    setPendingEmail("");
  };

  return {
    loading,
    error,
    isAuthenticated,
    user,
    login,
    clearAuthError,
    showActivationModal,
    pendingEmail,
    handleActivationConfirm,
    handleActivationCancel,
  };
};
