import { useCallback } from "react";
import toast from "react-hot-toast";

interface ToastOptions {
  duration?: number;
  position?:
    | "top-left"
    | "top-center"
    | "top-right"
    | "bottom-left"
    | "bottom-center"
    | "bottom-right";
}

export const useToast = () => {
  const showSuccess = useCallback((message: string, options?: ToastOptions) => {
    toast.success(message, {
      duration: options?.duration ?? 3000, // dùng ?? để nhận cả 0 hoặc Infinity
      position: options?.position || "top-right",
    });
  }, []);

  const showError = useCallback((message: string, options?: ToastOptions) => {
    toast.error(message, {
      duration: options?.duration ?? 5000,
      position: options?.position || "top-right",
    });
  }, []);

  const showWarning = useCallback((message: string, options?: ToastOptions) => {
    toast(message, {
      icon: "⚠️",
      duration: options?.duration ?? 4000,
      position: options?.position || "top-right",
    });
  }, []);

  const showInfo = useCallback((message: string, options?: ToastOptions) => {
    toast(message, {
      icon: "ℹ️",
      duration: options?.duration ?? 4000,
      position: options?.position || "top-right",
    });
  }, []);

  const showLoading = useCallback((message: string) => {
    return toast.loading(message);
  }, []);

  const dismiss = useCallback((toastId?: string) => {
    if (toastId) {
      toast.dismiss(toastId);
    } else {
      toast.dismiss();
    }
  }, []);

  return {
    showSuccess,
    showError,
    showWarning,
    showInfo,
    showLoading,
    dismiss,
  };
};
