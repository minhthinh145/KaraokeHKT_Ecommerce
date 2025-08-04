import React, { useState, useEffect } from "react";
import { Input } from "../ui/Input";
import { Button } from "../ui/Button";
import { useDispatch, useSelector } from "react-redux";
import { signInThunk, clearError } from "../../redux/auth/authSlice";
import type { RootState, AppDispatch } from "../../redux/store";
import { useToast } from "../../hooks/useToast";

export const LoginForm: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { loading, error, isAuthenticated } = useSelector(
    (state: RootState) => state.auth
  );
  const { showSuccess, showError } = useToast();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  // Hiển thị toast khi có lỗi
  useEffect(() => {
    if (error) {
      showError(error);
      dispatch(clearError());
    }
  }, [error, showError, dispatch]);

  // Hiển thị toast khi đăng nấuhập thành công
  useEffect(() => {
    if (isAuthenticated) {
      showSuccess("Đăng nhập thành công!", { duration: 3000 });
    }
  }, [isAuthenticated, showSuccess]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // Validation cơ bản
    if (!email || !password) {
      showError("Vui lòng điền đầy đủ thông tin!");
      return;
    }

    dispatch(signInThunk({ email, password }));
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
      <Button type="submit" fullWidth>
        {loading ? "Đang đăng nhập..." : "Đăng nhập"}
      </Button>
    </form>
  );
};
