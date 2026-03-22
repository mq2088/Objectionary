export interface GenerateRequest {
  concerns: string[];
  tone: 'formal' | 'moderate' | 'personal';
  personalNote?: string;
}

export interface GenerateResponse {
  text: string;
}

export interface ErrorResponse {
  error: string;
  limit?: 'hourly' | 'burst';
  message: string;
}

export type GenerateResult =
  | { ok: true; text: string }
  | { ok: false; error: string; rateLimitType?: 'hourly' | 'burst' };

export async function generateObjection(request: GenerateRequest): Promise<GenerateResult> {
  const response = await fetchWithRetry('/api/generate', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (response.ok) {
    const data: GenerateResponse = await response.json();
    return { ok: true, text: data.text };
  }

  if (response.status === 429) {
    const data: ErrorResponse = await response.json();
    return {
      ok: false,
      error: data.limit === 'burst'
        ? "You've reached the limit of 3 per minute. Please wait a moment."
        : "You've reached the limit of 5 per hour. Please try again later.",
      rateLimitType: data.limit as 'hourly' | 'burst' | undefined,
    };
  }

  return { ok: false, error: 'Something went wrong. Please try again in a moment.' };
}

async function fetchWithRetry(url: string, options: RequestInit, retries = 1): Promise<Response> {
  const response = await fetch(url, options);

  if (response.status === 503 && retries > 0) {
    await new Promise(resolve => setTimeout(resolve, 3000));
    return fetchWithRetry(url, options, retries - 1);
  }

  return response;
}
