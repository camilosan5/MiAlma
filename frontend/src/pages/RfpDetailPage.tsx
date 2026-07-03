import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { getRfpById } from '../api/rfps'
import { ApiError } from '../api/client'
import { StatusBadge } from '../components/StatusBadge'
import type { RfpWithProposalsDto } from '../types'

export function RfpDetailPage() {
  const { id } = useParams<{ id: string }>()
  const [rfp, setRfp] = useState<RfpWithProposalsDto | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (!id) return

    let cancelled = false
    setRfp(null)
    setError(null)

    getRfpById(id)
      .then((data) => {
        if (!cancelled) setRfp(data)
      })
      .catch((err) => {
        if (!cancelled) {
          setError(err instanceof ApiError ? err.message : 'Failed to load this RFP.')
        }
      })

    return () => {
      cancelled = true
    }
  }, [id])

  if (error) {
    return <p role="alert">{error}</p>
  }

  if (rfp === null) {
    return <p>Loading RFP…</p>
  }

  return (
    <div>
      <p>
        <Link to="/rfps">&larr; Back to RFPs</Link>
      </p>
      <h2>{rfp.title}</h2>
      <p>
        <strong>Agency:</strong> {rfp.agency}
      </p>
      <p>
        <strong>Deadline:</strong> {rfp.deadline}
      </p>
      <p>{rfp.description}</p>

      <h3>Proposals</h3>
      {rfp.proposals.length === 0 ? (
        <p>No proposals yet for this RFP.</p>
      ) : (
        <ul>
          {rfp.proposals.map((proposal) => (
            <li key={proposal.id}>
              {proposal.title} — <StatusBadge status={proposal.status} />
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
