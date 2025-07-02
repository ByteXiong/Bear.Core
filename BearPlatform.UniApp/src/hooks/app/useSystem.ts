/**
 * 系统信息
 * @description 除去uni官方不推荐使用的返回参数
 * @link https://uniapp.dcloud.net.cn/api/system/info.html
 */
export function useSystem() {
  const systemInfo = reactive<UniNamespace.GetSystemInfoResult>(uni.getSystemInfoSync());
  return {
    systemInfo,
  };
}
