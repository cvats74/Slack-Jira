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
    console.log('=== LOGIN DEBUG ===');
    console.log('URL:', axiosInstance.defaults.baseURL + '/auth/login');
    console.log('Sending:', dto);
    
    const response = await axiosInstance.post('/auth/login', dto);
    
    console.log('Full response:', response);
    console.log('response.data:', response.data);
    console.log('response.data.data:', response.data?.data);
    console.log('=== END DEBUG ===');
    
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
