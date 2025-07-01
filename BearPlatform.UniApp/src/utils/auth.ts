import { getCache, setCache } from '@/utils/cache';

export function getToken() {
  return getCache<string>('token') || null;
}

/** Clear auth storage */
export function clearAuthStorage() {
  setCache('token', null);
  setCache('refreshToken', null);
}
