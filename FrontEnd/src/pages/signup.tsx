import React from "react";
import { useNavigate } from "react-router-dom";
import { AuthLayout } from "../components/Auth/layout";
import { SignUpForm } from "../components/Auth/SignUpForm";

export const SignUpPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <AuthLayout
      title="Đăng ký tài khoản"
      subtitle="Tạo tài khoản mới để bắt đầu sử dụng"
      footerText="Đã có tài khoản?"
      footerLinkText="Đăng nhập ngay"
      onFooterLinkClick={() => navigate("/login")}
    >
      <SignUpForm />
    </AuthLayout>
  );
};
