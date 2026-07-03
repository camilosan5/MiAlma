import { apiFetch } from './client'
import type { CreateProposalRequestDto, ProposalDto, ProposalStatus, UpdateProposalRequestDto } from '../types'

export function getProposalsByRfp(rfpId: string, status?: ProposalStatus): Promise<ProposalDto[]> {
  const query = new URLSearchParams({ rfpId })
  if (status) query.set('status', status)

  return apiFetch<ProposalDto[]>(`/api/proposals?${query.toString()}`)
}

export function createProposal(data: CreateProposalRequestDto): Promise<ProposalDto> {
  return apiFetch<ProposalDto>('/api/proposals', {
    method: 'POST',
    body: JSON.stringify(data),
  })
}

export function updateProposal(id: string, data: UpdateProposalRequestDto): Promise<ProposalDto> {
  return apiFetch<ProposalDto>(`/api/proposals/${id}`, {
    method: 'PUT',
    body: JSON.stringify(data),
  })
}

export function changeProposalStatus(id: string, status: ProposalStatus): Promise<ProposalDto> {
  return apiFetch<ProposalDto>(`/api/proposals/${id}/status`, {
    method: 'PATCH',
    body: JSON.stringify({ status }),
  })
}
