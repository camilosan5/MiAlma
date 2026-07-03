import { apiFetch } from './client'
import type { RfpDto, RfpWithProposalsDto } from '../types'

export function getRfps(): Promise<RfpDto[]> {
  return apiFetch<RfpDto[]>('/api/rfps')
}

export function getRfpById(id: string): Promise<RfpWithProposalsDto> {
  return apiFetch<RfpWithProposalsDto>(`/api/rfps/${id}`)
}
