import { Outlet } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

export function Layout() {
  const { user, logout } = useAuth()

  return (
    <div>
      <header>
        <strong>MiAlma — Proposal Tracker</strong>
        {user && (
          <span>
            {' '}
            — {user.email} <button onClick={logout}>Log out</button>
          </span>
        )}
      </header>
      <hr />
      <Outlet />
    </div>
  )
}
