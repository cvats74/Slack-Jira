import axiosInstance from './axiosInstance';
import type { 
  LoginDto, 
  RegisterDto, 
  AuthResponseDto,
  ApiResponse 
} from '../types/auth.types';

export const authApi = {
  
  login: async (dto: LoginDto): Promise<AuthResponseDto> => {
    const response = await axiosInstance.post<ApiResponse<AuthResponseDto>>('/auth/login', dto);
    
    return response.data.data;
  },

  register: async (dto: RegisterDto): Promise<AuthResponseDto> => {
    const response = await axiosInstance.post<ApiResponse<AuthResponseDto>>('/auth/register', dto);
    
    return response.data.data;
  },
};
