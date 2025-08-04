import React from "react";
import { AuthLayout } from "../components/Auth/layout";
import { LoginForm } from "../components/Auth/LoginForm";
import { useNavigate } from "react-router-dom";

export const LoginPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <AuthLayout
      title="Đăng nhập"
      subtitle="Nhập email và mật khẩu của bạn để đăng nhập"
      footerText="Bạn chưa có tài khoản?"
      footerLinkText="Đăng ký ngay"
      onFooterLinkClick={() => navigate("/signup")}
      extraLinkText="Quên mật khẩu?"
      onExtraLinkClick={() => navigate("/forgot-password")}
    >
      <LoginForm />
    </AuthLayout>
  );
};
