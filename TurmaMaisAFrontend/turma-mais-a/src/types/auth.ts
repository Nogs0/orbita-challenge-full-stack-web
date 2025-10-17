
export interface AuthState {
  token: string | null;
  tokenExpiration: string | null;
  userFullname: string | null;
  organizationName: string | null;
}

export interface LoginResult {
  success: boolean;
  message?: string | null;
}