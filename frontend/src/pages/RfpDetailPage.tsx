import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { getRfpById } from '../api/rfps'
import { changeProposalStatus, createProposal, getProposalsByRfp, updateProposal } from '../api/proposals'
import { ApiError } from '../api/client'
import { StatusBadge } from '../components/StatusBadge'
import { StatusActions } from '../components/StatusActions'
import { ProposalForm } from '../components/ProposalForm'
import type { ProposalDto, ProposalStatus, RfpWithProposalsDto } from '../types'

const STATUS_OPTIONS: Array<ProposalStatus | 'All'> = ['All', 'Draft', 'InReview', 'Submitted', 'Won', 'Lost']
const EDITABLE_STATUSES: ProposalStatus[] = ['Draft', 'InReview']

export function RfpDetailPage() {
  const { id } = useParams<{ id: string }>()
  const [rfp, setRfp] = useState<RfpWithProposalsDto | null>(null)
  const [rfpError, setRfpError] = useState<string | null>(null)

  const [statusFilter, setStatusFilter] = useState<ProposalStatus | 'All'>('All')
  const [proposals, setProposals] = useState<ProposalDto[] | null>(null)
  const [proposalsError, setProposalsError] = useState<string | null>(null)
  const [reloadToken, setReloadToken] = useState(0)

  const [showCreateForm, setShowCreateForm] = useState(false)
  const [editingId, setEditingId] = useState<string | null>(null)

  useEffect(() => {
    if (!id) return

    let cancelled = false
    setRfp(null)
    setRfpError(null)

    getRfpById(id)
      .then((data) => {
        if (!cancelled) setRfp(data)
      })
      .catch((err) => {
        if (!cancelled) {
          setRfpError(err instanceof ApiError ? err.message : 'Failed to load this RFP.')
        }
      })

    return () => {
      cancelled = true
    }
  }, [id])

  useEffect(() => {
    if (!id) return

    let cancelled = false
    setProposals(null)
    setProposalsError(null)

    getProposalsByRfp(id, statusFilter === 'All' ? undefined : statusFilter)
      .then((data) => {
        if (!cancelled) setProposals(data)
      })
      .catch((err) => {
        if (!cancelled) {
          setProposalsError(err instanceof ApiError ? err.message : 'Failed to load proposals.')
        }
      })

    return () => {
      cancelled = true
    }
  }, [id, statusFilter, reloadToken])

  const handleCreate = async (title: string, content: string) => {
    if (!id) return

    await createProposal({ rfpId: id, title, content })
    setShowCreateForm(false)
    setReloadToken((token) => token + 1)
  }

  const handleUpdate = async (proposalId: string, title: string, content: string) => {
    await updateProposal(proposalId, { title, content })
    setEditingId(null)
    setReloadToken((token) => token + 1)
  }

  const handleStatusChange = async (proposalId: string, next: ProposalStatus) => {
    await changeProposalStatus(proposalId, next)
    setReloadToken((token) => token + 1)
  }

  if (rfpError) {
    return (
      <p className="alert" role="alert">
        {rfpError}
      </p>
    )
  }

  if (rfp === null) {
    return <p>Loading RFP…</p>
  }

  return (
    <div className="page">
      <p>
        <Link className="back-link" to="/rfps">
          &larr; Back to RFPs
        </Link>
      </p>
      <h2>{rfp.title}</h2>
      <p className="meta">
        {rfp.agency} — deadline: {rfp.deadline}
      </p>
      <p>{rfp.description}</p>

      <h3>Proposals</h3>
      <div className="toolbar">
        <label htmlFor="status-filter">Filter by status</label>
        <select
          id="status-filter"
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value as ProposalStatus | 'All')}
        >
          {STATUS_OPTIONS.map((status) => (
            <option key={status} value={status}>
              {status}
            </option>
          ))}
        </select>
      </div>

      {proposalsError && (
        <p className="alert" role="alert">
          {proposalsError}
        </p>
      )}
      {!proposalsError && proposals === null && <p>Loading proposals…</p>}
      {!proposalsError && proposals !== null && proposals.length === 0 && (
        <p>No proposals match this filter.</p>
      )}
      {!proposalsError && proposals !== null && proposals.length > 0 && (
        <ul className="card-list">
          {proposals.map((proposal) =>
            editingId === proposal.id ? (
              <li key={proposal.id} className="card">
                <ProposalForm
                  initialTitle={proposal.title}
                  initialContent={proposal.content}
                  submitLabel="Save changes"
                  onSubmit={(title, content) => handleUpdate(proposal.id, title, content)}
                  onCancel={() => setEditingId(null)}
                />
              </li>
            ) : (
              <li key={proposal.id} className="card proposal-card">
                <span className="proposal-card__title">{proposal.title}</span>
                <div className="proposal-card__actions">
                  <StatusBadge status={proposal.status} />
                  {EDITABLE_STATUSES.includes(proposal.status) && (
                    <button
                      className="btn btn-secondary btn-sm"
                      type="button"
                      onClick={() => setEditingId(proposal.id)}
                    >
                      Edit
                    </button>
                  )}
                  <StatusActions
                    status={proposal.status}
                    onTransition={(next) => handleStatusChange(proposal.id, next)}
                  />
                </div>
              </li>
            ),
          )}
        </ul>
      )}

      {showCreateForm ? (
        <ProposalForm
          submitLabel="Create proposal"
          onSubmit={handleCreate}
          onCancel={() => setShowCreateForm(false)}
        />
      ) : (
        <button className="btn btn-primary" type="button" onClick={() => setShowCreateForm(true)}>
          New proposal
        </button>
      )}
    </div>
  )
}
