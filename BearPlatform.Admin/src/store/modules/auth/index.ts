import { computed, reactive, ref } from 'vue';
import { useRoute } from 'vue-router';
import { defineStore } from 'pinia';
import { useRequest } from 'alova/client';
import type { JwtUserInfo, LoginParam, LoginToken } from '@/api/globals';
import { useRouterPush } from '@/hooks/common/router';
import { localStg } from '@/utils/storage';
import { encrypt } from '@/utils/rsaEncrypt';
import { SetupStoreId } from '@/enum';
import { $t } from '@/locales';
import { useRouteStore } from '../route';
import { useTabStore } from '../tab';
import { clearAuthStorage, getToken } from './shared';
import '@/api';

export const useAuthStore = defineStore(SetupStoreId.Auth, () => {
  const route = useRoute();
  const routeStore = useRouteStore();
  const tabStore = useTabStore();
  const { toLogin, redirectFromLogin } = useRouterPush(false);

  const token = ref(getToken());

  const userInfo: JwtUserInfo = reactive({
    user: {
      id: '',
      username: '',
      realName: '',
      avatar: '',
      email: ''
    },
    roles: [],
    dataScopes: []
  });

  /** is super role in static route */
  const isStaticSuper = computed(() => {
    const { VITE_AUTH_ROUTE_MODE, VITE_STATIC_SUPER_ROLE } = import.meta.env;

    return VITE_AUTH_ROUTE_MODE === 'static' && userInfo.roles?.includes(VITE_STATIC_SUPER_ROLE);
  });

  /** Is login */
  const isLogin = computed(() => Boolean(token.value));

  /** Reset auth store */
  async function resetStore() {
    const authStore = useAuthStore();

    clearAuthStorage();

    authStore.$reset();

    if (!route.meta.constant) {
      await toLogin();
    }

    tabStore.cacheTabs();
    routeStore.resetStore();
  }

  const { send: getUserInfo } = useRequest(
    () =>
      Apis.Login.get_getinfo({
        transform: ({ data }) => {
          Object.assign(userInfo, data);
          return true;
        }
      }),
    {
      immediate: false,
      force: true
      // async middleware(_, next) {
      //   if (userInfo.user?.id === 0) {
      //     await next();
      //   }
      //   return userInfo;
      // }
    }
  );

  const { send: login, loading: loginLoading } = useRequest(
    (loginData: LoginParam) =>
      Apis.Login.post_login({
        data: { ...loginData, password: encrypt(loginData.password) },
        transform: async res => {
          if (res.success) {
            const pass = await loginByToken(res.data);
            if (pass) {
              await routeStore.initAuthRoute();
              await redirectFromLogin(false);

              if (routeStore.isInitAuthRoute) {
                window.$notification?.success({
                  title: $t('page.login.common.loginSuccess'),
                  message: $t('page.login.common.welcomeBack', { userName: userInfo.user?.userName }),
                  duration: 4500
                });
              }
            }
          } else {
            resetStore();
          }
          return res;
        }
      }),
    {
      immediate: false
      // async middleware(_, next) {
      //   startLoading();
      //   await next();
      //   console.log('next', next);
      //   endLoading();
      // }
    }
  );

  // /**
  //  * Login
  //  *
  //  * @param userName User name
  //  * @param password Password
  //  * @param [redirect=true] Whether to redirect after login. Default is `true`
  //  */
  // async function login(userName: string, password: string, redirect = true) {
  //   startLoading();

  //   const { data: loginToken, error } = await fetchLogin(userName, password);

  //   if (!error) {
  //     const pass = await loginByToken(loginToken);

  //     if (pass) {
  //       await redirectFromLogin(redirect);

  //       window.$notification?.success({
  //         title: $t('page.login.common.loginSuccess'),
  //         message: $t('page.login.common.welcomeBack', { userName: userInfo.userName }),
  //         duration: 4500
  //       });
  //     }
  //   } else {
  //     resetStore();
  //   }

  //   endLoading();
  // }

  async function loginByToken(loginToken: LoginToken) {
    // 1. stored in the localStorage, the later requests need it in headers
    localStg.set('token', loginToken.accessToken || '');
    localStg.set('refreshToken', loginToken.refreshToken || '');
    // 2. get user info
    const pass = await getUserInfo();

    if (pass) {
      token.value = loginToken.accessToken || '';

      return true;
    }

    return false;
  }

  // async function getUserInfo() {
  //   const { data: info, error } = await fetchGetUserInfo();

  //   if (!error) {
  //     // update store
  //     Object.assign(userInfo, info);

  //     return true;
  //   }

  //   return false;
  // }

  async function initUserInfo() {
    const hasToken = getToken();

    if (hasToken) {
      const pass = await getUserInfo();

      if (!pass) {
        resetStore();
      }
    }
  }

  return {
    token,
    userInfo,
    isStaticSuper,
    isLogin,
    loginLoading,
    resetStore,
    login,
    initUserInfo
  };
});
