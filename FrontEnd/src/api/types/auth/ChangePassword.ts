export interface ChangePasswordDTO {
  oldPassword: string; // Mật khẩu hiện tại
  newPassword: string; // Mật khẩu mới
  confirmPassword: string; // Xác nhận mật khẩu
}
export interface ConfirmChangePasswordDTO {
  newPassword: string; // Mật khẩu mới (lần 2)
  otpCode: string; // Mã OTP từ email
}
