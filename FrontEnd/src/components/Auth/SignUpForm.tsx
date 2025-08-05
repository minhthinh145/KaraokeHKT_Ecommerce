import { Link, useNavigate } from "react-router-dom";
import { Input } from "../ui/Input";
import { Button } from "../ui/Button";
import { OtpVerificationModal } from "./OtpVerificationModal";
import { useSignUpForm } from "../../hooks/useSignUpForm";
import { useToast } from "../../hooks/useToast";

export const SignUpForm = () => {
  const navigate = useNavigate();
  const { showSuccess } = useToast();
  const {
    email,
    setEmail,
    username,
    setUsername,
    phoneNumber,
    setPhoneNumber,
    dateOfBirth,
    setDateOfBirth,
    password,
    setPassword,
    confirmPassword,
    setConfirmPassword,
    showOtpModal,
    setShowOtpModal,
    signupEmail,
    validationErrors,
    loading,
    handleSubmit,
  } = useSignUpForm();

  const handleOtpVerificationSuccess = () => {
    showSuccess("Tài khoản đã được xác thực thành công! Vui lòng đăng nhập.");
    // Navigate to login page after successful verification
    navigate("/login", { replace: true });
  };

  return (
    <>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          handleSubmit();
        }}
        className="space-y-4"
      >
        <Input
          label="Email"
          type="email"
          placeholder="Nhập email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        {validationErrors.email && (
          <p className="text-red-600 text-sm">{validationErrors.email}</p>
        )}

        <Input
          label="Họ và tên"
          type="text"
          placeholder="Nhập họ và tên"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        />
        {validationErrors.username && (
          <p className="text-red-600 text-sm">{validationErrors.username}</p>
        )}

        <Input
          label="Số điện thoại"
          type="tel"
          placeholder="Nhập số điện thoại"
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
          required
        />
        {validationErrors.phoneNumber && (
          <p className="text-red-600 text-sm">{validationErrors.phoneNumber}</p>
        )}

        <Input
          label="Ngày sinh"
          type="date"
          placeholder="Nhập ngày sinh"
          value={dateOfBirth}
          onChange={(e) => setDateOfBirth(e.target.value)}
          required
        />
        {validationErrors.dateOfBirth && (
          <p className="text-red-600 text-sm">{validationErrors.dateOfBirth}</p>
        )}

        <Input
          label="Mật khẩu"
          type="password"
          placeholder="Nhập mật khẩu"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        {validationErrors.password && (
          <p className="text-red-600 text-sm">{validationErrors.password}</p>
        )}

        <Input
          label="Xác nhận mật khẩu"
          type="password"
          placeholder="Nhập lại mật khẩu"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
        />
        {validationErrors.confirmPassword && (
          <p className="text-red-600 text-sm">
            {validationErrors.confirmPassword}
          </p>
        )}

        <Button type="submit" fullWidth>
          {loading ? "Đang đăng ký..." : "Đăng ký"}
        </Button>

        <div className="text-center">
          <span className="text-gray-600">Đã có tài khoản? </span>
          <Link
            to="/login"
            className="text-blue-600 hover:text-blue-800 font-medium"
          >
            Đăng nhập
          </Link>
        </div>
      </form>

      <OtpVerificationModal
        isOpen={showOtpModal}
        onClose={() => setShowOtpModal(false)}
        onVerificationSuccess={handleOtpVerificationSuccess}
        userEmail={signupEmail}
      />
    </>
  );
};
