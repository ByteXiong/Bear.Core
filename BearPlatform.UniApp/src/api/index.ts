import AdapterUniapp from '@alova/adapter-uniapp';
import { createAlova } from 'alova';

// import vueHook from 'alova/vue';
// import { mockAdapter } from '@/mock';
import qs from 'qs';
import { useUserStore } from '@/stores/modules/user';
import { getBaseUrl } from '@/utils/env';
import { createApis, withConfigType } from './createApis';

const BASE_URL = getBaseUrl();

interface ICodeMessage {
  [key: number]: string
}
enum ResultEnum {
  SUCCESS = 0,
  TIMEOUT = 401,
}
const serverCodeMessage: ICodeMessage = {
  200: '服务器成功返回请求的数据',
  401: '无用户信息,请登录',
  403: 'Forbidden',
  404: '网络请求错误,未找到该资源',
  500: '服务器发生错误，请检查服务器(Internal Server Error)',
  502: '网关错误(Bad Gateway)',
  503: '服务不可用，服务器暂时过载或维护(Service Unavailable)',
  504: '网关超时(Gateway Timeout)',
  547: '数据关联,删除失败',
};
console.log(BASE_URL);

// const modal = useModal();
export const alovaInstance = createAlova({
  baseURL: BASE_URL,
  //
  //   ...AdapterUniapp({
  //     /* #ifndef APP-PLUS */
  //     mockRequest: isUseMock() ? mockAdapter : undefined, // APP 平台无法使用mock
  //     /* #endif */
  //   }),
  ...AdapterUniapp(),
  beforeRequest: (method) => {
    const userStore = useUserStore();
    method.config.headers['api-version'] = 2.0;
    method.config.headers['Content-Type'] = 'application/json';
    // const { config } = method;
    // const ignoreAuth = !config.meta?.ignoreAuth;
    // const authorization = ignoreAuth ? userStore.token : null;
    // if (ignoreAuth && !authorization) {
    //   throw new Error('[请求错误]：未登录');
    // }
    method.config.headers.Authorization = userStore.token;
    method.config.params = qs.stringify(method.config.params, { arrayFormat: 'brackets' });
  },
  responded: {
    onSuccess: async (response) => {
      const { data: rawData } = response as UniNamespace.RequestSuccessCallbackResult;

      const { code, msg } = rawData as any;
      if (code === ResultEnum.SUCCESS) {
        return rawData as any;
      } else if (code === ResultEnum.TIMEOUT) {
        uni.showToast({
          title: '当前页面已失效，请重新登录',
          duration: 2000,
        });
        const userStore = useUserStore();
        userStore.logout();
      }

      Promise.reject(rawData);
      throw new Error(msg || serverCodeMessage[code] || '系统出错');

      // Promise.reject(rawData);
      // throw new Error(rawData);
    },
    onError: (err) => {
      if (err.response?.data?.code === ResultEnum.TIMEOUT) {
        uni.showToast({
          title: '当前页面已失效，请重新登录',
          duration: 2000,
        });
        // authStore.logout();
      } else {
        uni.showToast({
          title: err.response?.data?.msg || '系统出错',
          duration: 2000,
        });
        throw new Error(err.response?.data?.msg || '系统出错');
      }
      // return Promise.reject({ err, method });
    },
    onComplete: () => {
      // 处理请求完成逻辑
    },
  },
});

export const $$userConfigMap = withConfigType({});

const Apis = createApis(alovaInstance, $$userConfigMap);

export default Apis;
