import axiosInstance from './axiosInstance';
import type { 
  LoginDto, 
  RegisterDto, 
  AuthResponseDto,
  ApiResponse 
} from '../types/auth.types';

export const authApi = {
  
  login: async (dto: LoginDto): Promise<AuthResponseDto> => {
  try {
    console.log('Calling API with:', dto);
    console.log('Base URL:', axiosInstance.defaults.baseURL);
    
    const response = await axiosInstance.post<ApiResponse<AuthResponseDto>>('/auth/login', dto);
    
    console.log('API Response:', response);
    return response.data.data;
    
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (error: any) {
    console.log('API Error:', error);
    console.log('Error response:', error.response);
    console.log('Error message:', error.message);
    throw error;
  }
},

  register: async (dto: RegisterDto): Promise<AuthResponseDto> => {
    const response = await axiosInstance.post<ApiResponse<AuthResponseDto>>('/auth/register', dto);
    
    return response.data.data;
  },
};
