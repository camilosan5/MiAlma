import type { ProposalStatus } from '../types'

const LABELS: Record<ProposalStatus, string> = {
  Draft: 'Draft',
  InReview: 'In Review',
  Submitted: 'Submitted',
  Won: 'Won',
  Lost: 'Lost',
}

export function StatusBadge({ status }: { status: ProposalStatus }) {
  return <span className={`status-badge status-${status.toLowerCase()}`}>{LABELS[status]}</span>
}
