import axios from 'axios';

//create configured axios instance

const axiosInstance = axios.create({

    // .net api base URL
     baseURL: 'https://localhost:7293/api',

    headers : {
        'Content-Type' : 'application/json',

    },
});

//request interceptor

axiosInstance.interceptors.request.use(
  (config) => {
    // Get token from localStorage
    const token = localStorage.getItem('token');
    
    if (token) {
      // Attach JWT to every request automatically
     
      config.headers.Authorization = `Bearer ${token}`;
    }
    
    return config;
  },
  (error) => Promise.reject(error)
);

//response Interceptor
axiosInstance.interceptors.request.use(
    
    (response) => response,

    (error) => {
        if(error.response?.status === 401) {

            localStorage.removeItem('token');
            localStorage.removeItem('user');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;