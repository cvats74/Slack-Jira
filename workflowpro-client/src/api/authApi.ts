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
   
    const response = await axiosInstance.post('/auth/login', dto);
   
    return response.data.data;
    
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (error: any) {
    console.log('=== ERROR DEBUG ===');
    console.log('Error type:', error.constructor.name);
    console.log('Error message:', error.message);
    console.log('Error response status:', error.response?.status);
    console.log('Error response data:', error.response?.data);
    console.log('Is network error:', !error.response);
    console.log('=== END DEBUG ===');
    throw error;
  }
 },

  register: async (dto: RegisterDto): Promise<AuthResponseDto> => {
    const response = await axiosInstance.post<ApiResponse<AuthResponseDto>>('/auth/register', dto);
    
    return response.data.data;
  },
};
