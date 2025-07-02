import multiavatar from '@multiavatar/multiavatar';
import { random } from 'lodash-es';

const CHS_RANGE_START = 0x4E00; // 简体中文编码范围开始
const CHS_RANGE_END = 0x9FA5; // 简体中文编码范围结束

// 生成随机简体中文字符的函数
function getRandomChsChar() {
  // 生成 CHS_RANGE_START 和 CHS_RANGE_END 之间的随机整数
  const randomCharCode
    = Math.floor(Math.random() * (CHS_RANGE_END - CHS_RANGE_START + 1)) + CHS_RANGE_START;

  // 将随机整数转换为字符
  return String.fromCharCode(randomCharCode);
}

// 生成指定长度的随机简体中文字符串
export function getRandomChsString(length: number) {
  let result = '';
  for (let i = 0; i < length; i++) {
    result += getRandomChsChar();
  }
  return result;
}

/**
 * 随机 svg 图标
 */
export function getRandomIcon() {
  const svgCode = multiavatar(getRandomChsString(random(16, 32)));
  return `data:image/svg+xml;charset=utf-8,${encodeURIComponent(
    svgCode,
  )}`;
}
