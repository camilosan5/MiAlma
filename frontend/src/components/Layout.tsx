import { Outlet } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

export function Layout() {
  const { user, logout } = useAuth()

  return (
    <div>
      <header className="app-header">
        <span className="app-header__brand">MiAlma — Proposal Tracker</span>
        {user && (
          <div className="app-header__user">
            <span>{user.email}</span>
            <button className="btn btn-secondary btn-sm" onClick={logout}>
              Log out
            </button>
          </div>
        )}
      </header>
      <Outlet />
    </div>
  )
}
