import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import type React from "react";

interface ProtectedRouteProps {
    children : React.ReactNode;
}

// If not logged in → redirect to /login
// If logged in → show the page

function ProtectedRoute({children} : ProtectedRouteProps) {

    const {isAuthenticated, isLoading} = useAuth();

    if(isLoading) {
        return (
            <div style = {{ display : 'flex',
                justifyContent : 'center',
                alignContent : 'center',
                height : '100vh'
            }}> <p>Loading.....</p></div>
        )
    }

    //not logged in
    if(isAuthenticated){
        return <Navigate to = "/login" replace/>;
    }

    //logged in - show data
    return <>{children}</>
}

export default ProtectedRoute;