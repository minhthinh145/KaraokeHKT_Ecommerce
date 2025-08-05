import React, { useState } from "react";
import { Input } from "../ui/Input";
import { Button } from "../ui/Button";
import { useAuth } from "../../hooks/useSignInForm";
import { LoginOtpModal } from "./OtpVerification/LoginOtpModal";

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
  } = useAuth();

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

    // 🔥 Nếu cần activation và user confirm, show OTP modal
    if (result.needActivation && showActivationModal) {
      // Modal sẽ được hiển thị tự động
    }
  };

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
              label="Email"
              placeholder="Nhập email của bạn"
              type="email"
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

      <LoginOtpModal
        isOpen={showOtpModal}
        onClose={() => setShowOtpModal(false)}
        onVerificationSuccess={() => {
          setShowOtpModal(false);
          // Redirect to login page or show success message
          handleActivationConfirm();
        }}
        userEmail={pendingEmail}
      />
    </>
  );
};
