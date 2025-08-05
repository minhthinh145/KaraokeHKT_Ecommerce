import React, { useState, useEffect } from "react";
import { Input } from "../ui/Input";
import { Button } from "../ui/Button";
import { useDispatch, useSelector } from "react-redux";
import {
  signInThunk, // 🔥 Bỏ fetchProfileThunk vì signInThunk đã gọi rồi
  clearError,
} from "../../redux/auth/authSlice";
import type { RootState, AppDispatch } from "../../redux/store";
import { useToast } from "../../hooks/useToast";
import { useNavigate } from "react-router-dom";

export const LoginForm: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { loading, error, isAuthenticated } = useSelector(
    (state: RootState) => state.auth
  );
  const { showSuccess, showError } = useToast();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  // Hiển thị toast khi có lỗi (chỉ show lỗi đăng nhập, không show lỗi hệ thống)
  useEffect(() => {
    if (error) {
      // Chỉ show lỗi đăng nhập, không show lỗi hệ thống backend
      if (
        error ===
          "Sai thông tin đăng nhập hoặc tài khoản chưa được kích hoạt." ||
        error === "Đăng nhập thất bại" ||
        error === "Không thể lấy thông tin người dùng"
      ) {
        showError(error);
      }
      // Luôn clear error trong Redux
      dispatch(clearError());
    }
  }, [error, showError, dispatch]);

  // Hiển thị toast khi đăng nhập thành công
  useEffect(() => {
    if (isAuthenticated) {
      showSuccess("Đăng nhập thành công!", { duration: 3000 });
    }
  }, [isAuthenticated, showSuccess]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!email || !password) {
      showError("Vui lòng điền đầy đủ thông tin!");
      return;
    }

    const formData = { email, password };

    try {
      await dispatch(signInThunk(formData)).unwrap();
      navigate("/", { replace: true });
    } catch {
      // Luôn show 1 lỗi chung, không show lỗi hệ thống
      showError("Sai thông tin đăng nhập hoặc tài khoản chưa được kích hoạt.");
    }
  };

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      <Input
        label="Email"
        placeholder="Nhập email"
        type="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        required
      />

      <Input
        label="Mật khẩu"
        placeholder="Nhập mật khẩu"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required
      />
      <Button type="submit" fullWidth disabled={loading}>
        {loading ? "Đang đăng nhập..." : "Đăng nhập"}
      </Button>
    </form>
  );
};
