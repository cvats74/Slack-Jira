import { 
  createContext, 
  useContext, 
  useState, 
  useEffect,
  type ReactNode 
} from 'react';
import type { CurrentUser } from '../types/auth.types';

// STEP 1: Define what our context contains
// Like an interface in C#
interface AuthContextType {
  user: CurrentUser | null;  // current logged in user
  isAuthenticated: boolean;  // is user logged in?
  isLoading: boolean;        // checking auth status?
  login: (user: CurrentUser) => void;   // login function
  logout: () => void;                    // logout function
}

// STEP 2: Create the context
// Like creating a DI container
const AuthContext = createContext<AuthContextType | null>(null);

// STEP 3: Create the Provider

export function AuthProvider({ children }: { children: ReactNode }) {
  
  // Current user state — null means not logged in
  const [user, setUser] = useState<CurrentUser | null>(null);
  
  // True while we're checking if user is already logged in
  const [isLoading, setIsLoading] = useState(true);

  // STEP 4: On app startup, check if user was already logged in
  // Runs once when app first loads
  useEffect(() => {
    const savedUser = localStorage.getItem('user');
    const token = localStorage.getItem('token');

    if (savedUser && token) {
     
      // eslint-disable-next-line react-hooks/set-state-in-effect
      setUser(JSON.parse(savedUser));
    }
    
    // Done checking
    setIsLoading(false);
  }, []);

  // STEP 5: Login function
  // Called after successful API login
  const login = (userData: CurrentUser) => {
    // Save to state (updates UI immediately)
    setUser(userData);
    
    // Save to localStorage (persists on refresh)
    localStorage.setItem('token', userData.token);
    localStorage.setItem('user', JSON.stringify(userData));
  };

  // STEP 6: Logout function
  const logout = () => {
    // Clear state
    setUser(null);
    
    // Clear localStorage
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  };

  // STEP 7: Provide values to all child components
  return (
    <AuthContext.Provider value={{
      user,
      isAuthenticated: user !== null,
      isLoading,
      login,
      logout,
    }}>
      {children}
    </AuthContext.Provider>
  );
}

// STEP 8: Custom hook for easy access
// Instead of useContext(AuthContext) everywhere
// Just write useAuth()
// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const context = useContext(AuthContext);
  
  if (!context) {
    throw new Error(
      'useAuth must be used inside AuthProvider'
    );
  }
  
  return context;
}