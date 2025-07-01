/**
 * @description 获取当前平台
 */

export const platform = PLATFORM;

export function isH5() {
  return platform === 'h5';
}

export function isApp() {
  return platform === 'app';
}

export function isMp() {
  return platform.startsWith('mp-');
}
