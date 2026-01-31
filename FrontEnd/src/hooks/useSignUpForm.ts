import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import type { AppDispatch, RootState } from "../redux/store";
import { signUpThunk } from "../redux/auth/index";
import { useToast } from "./useToast";
import type { SignUpDTO } from "../api/types/auth/AuthDTO";

export const useSignUpForm = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { loading } = useSelector((state: RootState) => state.auth);
  const { showSuccess, showError } = useToast();

  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showOtpModal, setShowOtpModal] = useState(false);
  const [signupEmail, setSignupEmail] = useState("");
  const [validationErrors, setValidationErrors] = useState<{
    [key: string]: string;
  }>({});

  const validateForm = (): boolean => {
    const errors: { [key: string]: string } = {};
    if (!email.trim()) errors.email = "Email không được để trống";
    else if (!/\S+@\S+\.\S+/.test(email)) errors.email = "Email không hợp lệ";
    if (!username.trim()) errors.username = "Tên đăng nhập không được để trống";
    else if (username.length < 3)
      errors.username = "Tên đăng nhập phải có ít nhất 3 ký tự";
    if (!phoneNumber.trim())
      errors.phoneNumber = "Số điện thoại không được để trống";
    else if (!/^[0-9]{10,11}$/.test(phoneNumber.replace(/\s/g, "")))
      errors.phoneNumber = "Số điện thoại không hợp lệ (10-11 số)";
    if (!dateOfBirth) errors.dateOfBirth = "Ngày sinh không được để trống";
    else {
      const birthDate = new Date(dateOfBirth);
      const today = new Date();
      const age = today.getFullYear() - birthDate.getFullYear();
      if (age < 13) errors.dateOfBirth = "Bạn phải đủ 13 tuổi để đăng ký";
    }
    if (!password) errors.password = "Mật khẩu không được để trống";
    else if (password.length < 6)
      errors.password = "Mật khẩu phải có ít nhất 6 ký tự";
    if (!confirmPassword) errors.confirmPassword = "Vui lòng xác nhận mật khẩu";
    else if (password !== confirmPassword)
      errors.confirmPassword = "Mật khẩu xác nhận không khớp";
    setValidationErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (onSuccess?: () => void) => {
    if (!validateForm()) {
      showError("Vui lòng sửa các lỗi bên dưới");
      return;
    }
    const formData: SignUpDTO = {
      email,
      username,
      phoneNumber,
      dateOfBirth,
      password,
      confirmPassword,
    };
    try {
      const result = await dispatch(signUpThunk(formData)).unwrap();
      showSuccess(result.message || "Đăng ký thành công");
      setSignupEmail(email);
      setShowOtpModal(true);
      if (onSuccess) onSuccess();
    } catch (error: any) {
      showError(error.message || "Đăng ký thất bại");
    }
  };

  return {
    // State
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
    setSignupEmail,
    validationErrors,
    loading,
    // Actions
    handleSubmit,
  };
};
