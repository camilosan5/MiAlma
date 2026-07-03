import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './auth/AuthContext'
import { ProtectedRoute } from './auth/ProtectedRoute'
import { Layout } from './components/Layout'
import { LoginPage } from './pages/LoginPage'
import { RfpDetailPage } from './pages/RfpDetailPage'
import { RfpListPage } from './pages/RfpListPage'

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route element={<ProtectedRoute />}>
            <Route element={<Layout />}>
              <Route path="/rfps" element={<RfpListPage />} />
              <Route path="/rfps/:id" element={<RfpDetailPage />} />
            </Route>
          </Route>
          <Route path="*" element={<Navigate to="/rfps" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  )
}

export default App
