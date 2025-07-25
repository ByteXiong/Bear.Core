import { createAlova } from 'alova';
import { axiosRequestAdapter } from '@sa/alova/adapter-axios';
import vueHook from 'alova/vue';
// import defaultSettings from '@/settings';
// import { useUserStoreHook } from '@/store/modules/user';
import type { Action } from 'element-plus';
import { ElMessageBox } from 'element-plus';
import { localStg } from '@/utils/storage';
import { useAuthStore } from '@/store/modules/auth';
import { getServiceBaseURL } from '@/utils/service';
import { createApis, withConfigType } from './createApis';
const isHttpProxy = import.meta.env.DEV && import.meta.env.VITE_HTTP_PROXY === 'Y';
const { baseURL } = getServiceBaseURL(import.meta.env, isHttpProxy);

type ICodeMessage = {
  [key: number]: string;
};
enum ResultEnum {
  SUCCESS = 0,
  TIMEOUT = 401
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
  547: '数据关联,删除失败'
};
// const modal = useModal();
export const alovaInstance = createAlova({
  baseURL,
  statesHook: vueHook,
  requestAdapter: axiosRequestAdapter(),
  beforeRequest: method => {
    method.config.headers.Authorization = `Bearer ${localStg.get('token')}`;
    method.config.headers['api-version'] = 1.0;
    method.config.headers['Content-Type'] = 'application/json';
  },
  responded: {
    onSuccess: async response => {
      const { data: rawData } = response;

      if (rawData.code === ResultEnum.SUCCESS) {
        return rawData as any;
      } else if (rawData.code === ResultEnum.TIMEOUT) {
        ElMessageBox.confirm('当前页面已失效，请重新登录', '提示', {
          confirmButtonText: '确认',
          type: 'warning',
          showCancelButton: false,
          closeOnClickModal: false,

          callback: (action: Action) => {
            if (action === 'confirm') {
              const authStore = useAuthStore();
              authStore.resetStore();
            }
          }
        });
      }
      window.$message?.error(rawData.msg || serverCodeMessage[rawData.code] || '系统出错');
      Promise.reject(rawData);
      throw new Error(rawData.msg || serverCodeMessage[rawData.code] || '系统出错');

      // Promise.reject(rawData);
      // throw new Error(rawData);
    },
    onError: err => {
      if (err.response?.data?.code === ResultEnum.TIMEOUT) {
        ElMessageBox.confirm('当前页面已失效，请重新登录', '提示', {
          confirmButtonText: '确认',
          type: 'warning',
          showCancelButton: false,
          closeOnClickModal: false,

          callback: (action: Action) => {
            if (action === 'confirm') {
              const authStore = useAuthStore();
              authStore.resetStore();
            }
          }
        });
      } else {
        window.$message?.error(err.response?.data?.msg || '系统出错');
        throw new Error(err.response?.data?.msg || '系统出错');
      }
      // return Promise.reject({ err, method });
    },
    onComplete: () => {
      // 处理请求完成逻辑
    }
  }
});

export const $$userConfigMap = withConfigType({});

const Apis = createApis(alovaInstance, $$userConfigMap);

export default Apis;
