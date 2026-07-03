const BASE_URL = import.meta.env.VITE_API_URL as string

export const TOKEN_STORAGE_KEY = 'mialma_token'

export class ApiError extends Error {
  status: number

  constructor(status: number, message: string) {
    super(message)
    this.status = status
  }
}

interface ApiFetchOptions extends RequestInit {
  auth?: boolean
}

export async function apiFetch<T>(path: string, options: ApiFetchOptions = {}): Promise<T> {
  const { auth = true, headers, ...rest } = options

  const finalHeaders: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(headers as Record<string, string>),
  }

  if (auth) {
    const token = localStorage.getItem(TOKEN_STORAGE_KEY)
    if (token) {
      finalHeaders['Authorization'] = `Bearer ${token}`
    }
  }

  const response = await fetch(`${BASE_URL}${path}`, {
    ...rest,
    headers: finalHeaders,
  })

  if (response.status === 204) {
    return undefined as T
  }

  const isJson = response.headers.get('content-type')?.includes('application/json')
  const body = isJson ? await response.json() : undefined

  if (!response.ok) {
    const message = body?.error ?? `Request failed with status ${response.status}`
    throw new ApiError(response.status, message)
  }

  return body as T
}
