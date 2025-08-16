import React, { useEffect, useRef, useState } from "react";
import { Input } from "../ui/Input";
import { Button } from "../ui/Button";
import { useSignInForm } from "../../hooks/useSignInForm"; // 🔥 SỬA: Import đúng hook
import { LoginOtpModal } from "./OtpVerification/LoginOtpModal";
import { useAuth } from "../../hooks/auth/useAuth";

export const LoginForm: React.FC = () => {
  const {
    login,
    loading,
    error,
    clearAuthError,
    showActivationModal,
    pendingEmail,
    handleActivationConfirm,
    handleActivationCancel,
  } = useSignInForm(); // 🔥 SỬA: Dùng useSignInForm
  const { userRole, navigateToDefaultRoute } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showOtpModal, setShowOtpModal] = useState(false);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (loading) return;

    const result = await login({
      email: email.trim(),
      password: password.trim(),
    });
    if (result.success) {
    }
  };
  const navigatedRef = useRef(false);

  useEffect(() => {
    if (userRole && !navigatedRef.current) {
      navigatedRef.current = true;
      navigateToDefaultRoute();
    }
  }, [userRole, navigateToDefaultRoute]);
  // 🔥 Handle activation confirmation
  const handleActivationConfirmAction = () => {
    const emailForOtp = handleActivationConfirm();
    setShowOtpModal(true);
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
    if (error) clearAuthError();
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
    if (error) clearAuthError();
  };

  return (
    <>
      <div className="space-y-6">
        <form onSubmit={handleSubmit}>
          <div className="space-y-4">
            <Input
              label="Tên đăng nhập"
              placeholder="Nhập tên đăng nhập của bạn"
              type="text"
              value={email}
              onChange={handleEmailChange}
              required
            />

            <Input
              label="Mật khẩu"
              placeholder="Nhập mật khẩu"
              type="password"
              value={password}
              onChange={handlePasswordChange}
              required
            />

            <Button
              type="submit"
              fullWidth
              disabled={loading || !email.trim() || !password.trim()}
              variant="primary"
            >
              {loading ? (
                <span className="flex items-center justify-center gap-2">
                  <span className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin"></span>
                  Đang đăng nhập...
                </span>
              ) : (
                "Đăng nhập"
              )}
            </Button>
          </div>
        </form>
      </div>

      {/* 🔥 Activation Modal */}
      {showActivationModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 max-w-md w-full mx-4">
            <h3 className="text-lg font-semibold mb-4">Kích hoạt tài khoản</h3>
            <p className="text-gray-600 mb-6">
              Tài khoản của bạn chưa được kích hoạt. Bạn có muốn gửi lại mã OTP
              để kích hoạt không?
            </p>
            <div className="flex gap-3 justify-end">
              <Button variant="secondary" onClick={handleActivationCancel}>
                Hủy
              </Button>
              <Button variant="primary" onClick={handleActivationConfirmAction}>
                Đồng ý
              </Button>
            </div>
          </div>
        </div>
      )}

      {/* 🔥 OTP Modal */}
      <LoginOtpModal
        isOpen={showOtpModal}
        onClose={() => setShowOtpModal(false)}
        onVerificationSuccess={() => {
          setShowOtpModal(false);
          handleActivationConfirm();
        }}
        userEmail={pendingEmail}
      />
    </>
  );
};
