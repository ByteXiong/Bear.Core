import type { UserInfoModel } from '@/services/model/userModel';
import { request } from '@/utils/http';

enum Api {
  GET_USER_INFO = '/users',
}

/**
 * 获取用户信息
 */
export function getUserInfoApi() {
  return request.Get<UserInfoModel>(Api.GET_USER_INFO);
}
