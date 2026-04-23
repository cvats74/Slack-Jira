import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import toast from 'react-hot-toast';
import { useAuth } from '../../context/AuthContext';
import { authApi } from '../../api/authApi';
import { type LoginDto } from '../../types/auth.types';

function LoginPage() {
  // Navigation hook — like RedirectToAction in MVC
  const navigate = useNavigate();
  
  // Get login function from auth context
  const { login } = useAuth();

  // Form state — what user is typing
  const [formData, setFormData] = useState<LoginDto>({
    email: '',
    password: '',
  });

  // Loading state — disable button while API call runs
  const [isLoading, setIsLoading] = useState(false);

  // Error state — show validation errors
  const [errors, setErrors] = useState<string[]>([]);

  // Handle input changes
  // Called every time user types in a field
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { name, value } = e.target;
    
    // Update only the field that changed
    // ...formData = keep all other fields
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    // Clear errors when user starts typing
    setErrors([]);
  };

  // Handle form submission
  const handleSubmit = async (
    e: React.FormEvent
  ) => {
    // Prevent default browser form submission
    // (page reload) — we handle it ourselves
    e.preventDefault();

    // Basic client-side validation
    if (!formData.email || !formData.password) {
      setErrors(['Please fill in all fields']);
      return;
    }

    setIsLoading(true);

    try {
      // Call your .NET API
      const response = await authApi.login(formData);

      // Save user to context + localStorage
      login({
        email: response.email,
        fullName: response.fullName,
        role: response.role,
        token: response.token,
      });

      // Show success notification
      toast.success(`Welcome back, ${response.fullName}!`);

      // Redirect to projects page
      navigate('/projects');
     // eslint-disable-next-line @typescript-eslint/no-explicit-any
    } catch (error: any) {
      // Extract error message from API response
      const message = 
        error.response?.data?.message || 
        'Login failed. Please try again.';
      
      setErrors([message]);
      toast.error(message);
      
    } finally {
      // Always stop loading, success or fail
      setIsLoading(false);
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.card}>
        
        {/* Header */}
        <div style={styles.header}>
          <h1 style={styles.title}>WorkFlow Pro</h1>
          <p style={styles.subtitle}>Sign in to your account</p>
        </div>

        {/* Error Messages */}
        {errors.length > 0 && (
          <div style={styles.errorBox}>
            {errors.map((error, index) => (
              <p key={index} style={styles.errorText}>
                {error}
              </p>
            ))}
          </div>
        )}

        {/* Login Form */}
        <form onSubmit={handleSubmit} style={styles.form}>
          
          {/* Email Field */}
          <div style={styles.fieldGroup}>
            <label style={styles.label}>
              Email Address
            </label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              placeholder="email"
              style={styles.input}
              disabled={isLoading}
              autoComplete="off"
            />
          </div>

          {/* Password Field */}
          <div style={styles.fieldGroup}>
            <label style={styles.label}>
              Password
            </label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="password"
              style={styles.input}
              disabled={isLoading}
              autoComplete="off"
            />
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            disabled={isLoading}
            style={{
              ...styles.button,
              opacity: isLoading ? 0.7 : 1,
              cursor: isLoading ? 'not-allowed' : 'pointer'
            }}
          >
            {isLoading ? 'Signing in...' : 'Sign In'}
          </button>

        </form>

        {/* Register Link */}
        <p style={styles.linkText}>
          Don't have an account?{' '}
          <Link to="/register" style={styles.link}>
            Create one
          </Link>
        </p>

      </div>
    </div>
  );
}

// Inline styles — we'll move to CSS modules later
const styles = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#f8fafc',
    padding: '20px',
  },
  card: {
    backgroundColor: 'white',
    borderRadius: '12px',
    padding: '40px',
    width: '100%',
    maxWidth: '400px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.07)',
  },
  header: {
    textAlign: 'center' as const,
    marginBottom: '32px',
  },
  title: {
    fontSize: '28px',
    fontWeight: '700',
    color: '#1e293b',
    marginBottom: '8px',
  },
  subtitle: {
    fontSize: '14px',
    color: '#64748b',
  },
  errorBox: {
    backgroundColor: '#fef2f2',
    border: '1px solid #fecaca',
    borderRadius: '8px',
    padding: '12px',
    marginBottom: '16px',
  },
  errorText: {
    color: '#dc2626',
    fontSize: '14px',
  },
  form: {
    display: 'flex',
    flexDirection: 'column' as const,
    gap: '20px',
  },
  fieldGroup: {
    display: 'flex',
    flexDirection: 'column' as const,
    gap: '6px',
  },
  label: {
    fontSize: '14px',
    fontWeight: '500',
    color: '#374151',
  },
  input: {
    padding: '10px 14px',
    border: '1px solid #d1d5db',
    borderRadius: '8px',
    fontSize: '14px',
    outline: 'none',
    width: '100%',
  },
  button: {
    backgroundColor: '#3b82f6',
    color: 'white',
    border: 'none',
    borderRadius: '8px',
    padding: '12px',
    fontSize: '15px',
    fontWeight: '600',
    width: '100%',
    marginTop: '8px',
  },
  linkText: {
    textAlign: 'center' as const,
    marginTop: '24px',
    fontSize: '14px',
    color: '#64748b',
  },
  link: {
    color: '#3b82f6',
    textDecoration: 'none',
    fontWeight: '500',
  },
};

export default LoginPage;