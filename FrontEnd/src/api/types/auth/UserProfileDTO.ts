export interface UserProfileDTO {
  userName: string;
  email: string;
  phone: string;
  birthDate: string | null;
  loaiTaiKhoan: string;
  birthDateFormatted?: string;
}
