import process from 'node:process';

const currentPlatform = process.env.UNI_PLATFORM;
const isH5 = currentPlatform === 'h5';
const isApp = currentPlatform === 'app';
const isMp = !isH5 && !isApp;
const isWeixinMp = currentPlatform === 'mp-weixin';

export {
  currentPlatform,
  isApp,
  isH5,
  isMp,
  isWeixinMp,
};
