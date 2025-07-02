import type { ProxyOptions } from 'vite';

/**
 * Configure according to the proxy list
 * @param proxyList
 */
export function resolveProxy(proxyList: [string, string][] = []) {
  const proxy: Record<string, ProxyOptions> = {};
  for (const [prefix, target] of proxyList) {
    const isHttps = /^https:\/\//.test(target);
    proxy[prefix] = {
      target,
      changeOrigin: true,
      ws: true,
      rewrite: path => path.replace(new RegExp(`^${prefix}`), ''),
      // https is require secure=false
      ...(isHttps ? { secure: false } : {}),
    };
  }
  return proxy;
}
