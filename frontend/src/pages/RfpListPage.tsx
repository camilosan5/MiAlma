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
    return <p role="alert">{error}</p>
  }

  if (rfps === null) {
    return <p>Loading RFPs…</p>
  }

  if (rfps.length === 0) {
    return <p>No RFPs available yet.</p>
  }

  return (
    <div>
      <h2>RFPs</h2>
      <ul>
        {rfps.map((rfp) => (
          <li key={rfp.id}>
            <Link to={`/rfps/${rfp.id}`}>{rfp.title}</Link> — {rfp.agency} (deadline: {rfp.deadline})
          </li>
        ))}
      </ul>
    </div>
  )
}
