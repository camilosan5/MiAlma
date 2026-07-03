import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './auth/AuthContext'
import { ProtectedRoute } from './auth/ProtectedRoute'
import { Layout } from './components/Layout'
import { LoginPage } from './pages/LoginPage'

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route element={<ProtectedRoute />}>
            <Route element={<Layout />}>
              <Route path="/rfps" element={<p>RFPs </p>} />
            </Route>
          </Route>
          <Route path="*" element={<Navigate to="/rfps" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App
