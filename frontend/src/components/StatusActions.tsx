import { useState } from 'react'
import type { ProposalStatus } from '../types'

const NEXT_STATUSES: Record<ProposalStatus, ProposalStatus[]> = {
  Draft: ['InReview'],
  InReview: ['Submitted'],
  Submitted: ['Won', 'Lost'],
  Won: [],
  Lost: [],
}

interface StatusActionsProps {
  status: ProposalStatus
  onTransition: (next: ProposalStatus) => Promise<void>
}

export function StatusActions({ status, onTransition }: StatusActionsProps) {
  const [error, setError] = useState<string | null>(null)
  const [submitting, setSubmitting] = useState<ProposalStatus | null>(null)

  const nextOptions = NEXT_STATUSES[status]
  if (nextOptions.length === 0) return null

  const handleClick = async (next: ProposalStatus) => {
    setError(null)
    setSubmitting(next)
    try {
      await onTransition(next)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update status.')
    } finally {
      setSubmitting(null)
    }
  }

  return (
    <span>
      {nextOptions.map((next) => (
        <button
          key={next}
          type="button"
          onClick={() => handleClick(next)}
          disabled={submitting !== null}
        >
          {submitting === next ? 'Updating…' : `Mark as ${next}`}
        </button>
      ))}
      {error && <span role="alert"> {error}</span>}
    </span>
  )
}
