const configuredBaseUrl = import.meta.env.VITE_API_BASE_URL
const API_BASE_URL = (configuredBaseUrl || 'http://localhost:5209').replace(/\/$/, '')

async function request(path, options = {}) {
  let response

  try {
    response = await fetch(`${API_BASE_URL}${path}`, options)
  } catch {
    throw new Error('We could not reach the lending service. Please check that the backend is running.')
  }

  const contentType = response.headers.get('content-type') || ''
  const body = contentType.includes('application/json') ? await response.json() : null

  if (!response.ok) {
    throw new Error(getErrorMessage(body, response.status))
  }

  return body
}

function getErrorMessage(body, status) {
  if (status === 400 && body?.errors) {
    const messages = Object.values(body.errors).flat()
    return messages[0] || 'Please check the application details and try again.'
  }

  return 'The lending service could not complete your request. Please try again.'
}

export function submitLoanApplication(application) {
  return request('/api/loans/applications', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(application),
  })
}

export function getLoanStatistics() {
  return request('/api/loans/statistics')
}
