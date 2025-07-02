import { ResultEnum } from '@/enums/httpEnum';
import { createMock } from '@/mock/utils';
import { getRandomChsString } from '@/utils/character';
import { defineMock } from '@alova/mock';
import multiavatar from '@multiavatar/multiavatar';
import { join, random, sampleSize } from 'lodash-es';

function createRandomToken(len = 36 * 6) {
  const token = join(sampleSize('0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ._-', len), '');
  return `eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.${token}`;
}

export const authMocks = defineMock({
  // 登录
  '[POST]/api/login': (params) => {
    const { email, password } = params.data;
    if (email === 'uni-app@test.com' && (password === 'Vue3_Ts_Vite' || password === '123456')) {
      const token = createRandomToken();
      return createMock({ data: { token } });
    }
    return createMock({ data: [], code: ResultEnum.FAIL, message: '邮箱或密码错误' });
  },
  // 获取用户信息
  '[GET]/api/users': () => {
    const generateNicknames = getRandomChsString(random(2, 6));
    const svgCode = multiavatar(generateNicknames);
    const base64SVG = `data:image/svg+xml;charset=utf-8,${encodeURIComponent(
      svgCode,
    )}`;

    return createMock({
      data: {
        id: 1,
        nickname: generateNicknames,
        avatar: base64SVG,
        email: 'uni-app@test.com',
      },
    });
  },
});
