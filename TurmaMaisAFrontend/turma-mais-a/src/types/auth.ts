export interface AuthState {
  token: string | null;
  tokenExpiration: string | null;
  userFullname: string | null;
  organizationName: string | null;
  isLoading: boolean;
  error: string | null;
}

export interface AuthResultDto {
  success: boolean;
  message?: string | null;
}

export interface LoginDto {
  username: string;
  password: string;
}

export interface RegisterDto {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
  organizationName: string;
}