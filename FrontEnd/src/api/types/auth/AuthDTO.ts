export interface SignUpDTO {
  email: string;
  username: string;
  phoneNumber: string;
  dateOfBirth: string;
  password: string;
  confirmPassword: string;
}

export interface SignInDTO {
  email: string;
  password: string;
}

export interface RefreshTokenRequestDTO {
  RefreshToken: string;
}

export interface TokenResponseDTO {
  accessToken: string;
  refreshToken: string;
}
