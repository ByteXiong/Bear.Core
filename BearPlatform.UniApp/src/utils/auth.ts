import { TOKEN_KEY } from '@/enums/cacheEnum';
import { getCache, setCache } from '@/utils/cache';

const authenticationScheme = 'Bearer';

export function getToken() {
  return getCache<string>(TOKEN_KEY) || null;
}

export function getAuthorization() {
  const token = getToken();
  return token ? `${authenticationScheme} ${token}` : null;
}

export function setToken(token: string) {
  return setCache(TOKEN_KEY, token);
}

export function removeToken() {
  return setCache(TOKEN_KEY, null);
}

// 是否登录
export function isLogin() {
  return !!getToken();
}
