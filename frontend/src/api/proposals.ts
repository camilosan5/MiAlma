import { apiFetch } from './client'
import type { ProposalDto, ProposalStatus } from '../types'

export function getProposalsByRfp(rfpId: string, status?: ProposalStatus): Promise<ProposalDto[]> {
  const query = new URLSearchParams({ rfpId })
  if (status) query.set('status', status)

  return apiFetch<ProposalDto[]>(`/api/proposals?${query.toString()}`)
}
