export type ProposalStatus = 'Draft' | 'InReview' | 'Submitted' | 'Won' | 'Lost'

export interface RfpDto {
  id: string
  title: string
  agency: string
  description: string
  deadline: string
  createdAt: string
}

export interface ProposalDto {
  id: string
  rfpId: string
  ownerId: string
  title: string
  content: string
  status: ProposalStatus
  createdAt: string
  updatedAt: string
}

export interface RfpWithProposalsDto extends RfpDto {
  proposals: ProposalDto[]
}

export interface LoginRequestDto {
  email: string
  password: string
}

export interface RegisterRequestDto {
  email: string
  password: string
}

export interface LoginResponseDto {
  token: string
  expiresAtUtc: string
  userId: string
  email: string
}

export interface CreateProposalRequestDto {
  rfpId: string
  title: string
  content: string
}

export interface UpdateProposalRequestDto {
  title: string
  content: string
}

export interface ChangeProposalStatusRequestDto {
  status: ProposalStatus
}
