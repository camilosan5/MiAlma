import { apiFetch } from './client'
import type { LoginRequestDto, LoginResponseDto, RegisterRequestDto } from '../types'

export function login(data: LoginRequestDto): Promise<LoginResponseDto> {
  return apiFetch<LoginResponseDto>('/api/auth/login', {
    method: 'POST',
    body: JSON.stringify(data),
    auth: false,
  })
}

export function register(data: RegisterRequestDto): Promise<LoginResponseDto> {
  return apiFetch<LoginResponseDto>('/api/auth/register', {
    method: 'POST',
    body: JSON.stringify(data),
    auth: false,
  })
}
