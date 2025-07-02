import type { JwtUserInfo, LoginParam, LoginToken } from '@/api/globals';
import { defineStore } from 'pinia';
import { TOKEN_KEY } from '@/enums/cacheEnum';
import { getToken } from '@/utils/auth';
import { removeCache, setCache } from '@/utils/cache';
import { encrypt } from '@/utils/rsaEncrypt';
import '@/api';

export const useUserStore = defineStore('UserStore', () => {
  const token = ref<string | null>(getToken());
  const userInfo = ref<JwtUserInfo | null>(null);

  // 是否登录
  const loggedIn = computed(() => !!token.value);

  // 登录
  const { send: login } = useRequest((loginData: LoginParam) =>
    Apis.Login.post_login({
      data: { ...loginData, password: encrypt(loginData.password) },
      transform: async (res) => {
        if (res.success) {
          await loginByToken(res.data);
        }
        return res;
      },
    }), { immediate: false, force: true });

  // 获取用户信息
  const { send: getUserInfo } = useRequest(
    () =>
      Apis.Login.get_getinfo({
        transform: ({ data }) => {
          userInfo.value = data;
          return true;
        },
      }),
    {
      immediate: false,
      force: true,
      // async middleware(_, next) {
      //   if (userInfo.user?.id === 0) {
      //     await next();
      //   }
      //   return userInfo;
      // }
    },
  );

  // 登出
  // const { send: sendLogout } = useRequest(logoutApi, { immediate: false });
  async function logout() {
    try {
      // await sendLogout();
      removeCache('token');
      userInfo.value = null;
      token.value = null;
    } catch (err: any) {
      throw err;
    }
  }
  async function loginByToken(loginToken: LoginToken) {
    // 1. stored in the localStorage, the later requests need it in headers
    token.value = `${loginToken.tokenType} ${loginToken.accessToken}` || '';
    setCache('token', token.value);
    setCache('refreshToken', loginToken.refreshToken || '');
    // 2. get user info
    const pass = await getUserInfo();
    if (pass) {
      return true;
    } else {
      token.value = null;
      setCache('token', null);
      setCache('refreshToken', null);
    }

    return false;
  }
  // 初始化
  async function initUserInfo() {
    const hasToken = getToken();
    console.log('token', hasToken);

    if (hasToken) {
      const pass = await getUserInfo();
      if (!pass) {
        logout();
      }
    }
  }

  return {
    token,
    userInfo,
    loggedIn,
    login,
    logout,
    getUserInfo,
    initUserInfo,
  };
});
