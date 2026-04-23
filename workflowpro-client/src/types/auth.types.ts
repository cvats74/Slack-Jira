// These mirror your .NET DTOs exactly
// C# RegisterDto ↔ RegisterDto here

export interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  organizationName: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponseDto {
  token: string;
  refreshToken: string;
  email: string;
  fullName: string;
  role: string;
  tokenExpiry: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: string[];
  statusCode: number;
}
// Current logged in user shape
export interface CurrentUser {
  email: string;
  fullName: string;
  role: string;
  token: string;
}