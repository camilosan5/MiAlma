import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { getRfps } from '../api/rfps'
import { ApiError } from '../api/client'
import type { RfpDto } from '../types'

export function RfpListPage() {
  const [rfps, setRfps] = useState<RfpDto[] | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    getRfps()
      .then((data) => {
        if (!cancelled) setRfps(data)
      })
      .catch((err) => {
        if (!cancelled) {
          setError(err instanceof ApiError ? err.message : 'Failed to load RFPs.')
        }
      })

    return () => {
      cancelled = true
    }
  }, [])

  if (error) {
    return (
      <p className="alert" role="alert">
        {error}
      </p>
    )
  }

  if (rfps === null) {
    return <p>Loading RFPs…</p>
  }

  if (rfps.length === 0) {
    return <p>No RFPs available yet.</p>
  }

  return (
    <div className="page">
      <h2>RFPs</h2>
      <ul className="card-list">
        {rfps.map((rfp) => (
          <li key={rfp.id} className="card">
            <Link className="rfp-card__title" to={`/rfps/${rfp.id}`}>
              {rfp.title}
            </Link>
            <p className="rfp-card__meta">
              {rfp.agency} — deadline: {rfp.deadline}
            </p>
          </li>
        ))}
      </ul>
    </div>
  )
}
