import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/common/ProtectedRoute';

// Pages (we build these next)
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import ProjectsPage from './pages/projects/ProjectsPage';

function App(){
  return 
  <BrowserRouter>

  <AuthProvider>

    <Toaster position="top-right" />

    <Routes>
      <Route path='/login' element = {<LoginPage/>} />
      <Route path = '/register' element = {<RegisterPage/>} />
      
      <Route path = '/projects' element = {
        <ProtectedRoute>
          <ProjectsPage/>
        </ProtectedRoute>
      } />

      element={<Navigate to="/projects" replace />} 
    </Routes>
  </AuthProvider>
  </BrowserRouter>
}

export default App;