import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import type React from "react";

interface ProtectedRouteProps {
  children: React.ReactNode;
}

function ProtectedRoute({ children }: ProtectedRouteProps) {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return (
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh'
      }}>
        <p>Loading...</p>
      </div>
    );
  }

  // NOT authenticated → redirect to login
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  // Authenticated → show the page
  return <>{children}</>;
}

export default ProtectedRoute;