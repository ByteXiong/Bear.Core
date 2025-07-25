import { getCurrentInstance, h } from 'vue';
import * as UI from 'element-plus';
import dayjs from 'dayjs';
import { $t } from '@/locales';
// import * as El from '@/api/apiEls';
import * as Enum from '@/api/apiEnums';
import { useAuth } from '@/hooks/business/auth';
const customRender = (str:  Record<string, any>|undefined) => {
  const currentCpn = getCurrentInstance();

  // eslint-disable-next-line consistent-this, @typescript-eslint/no-unused-vars
  const that = currentCpn?.exposed;
  Object.defineProperty(window, 'that', {
    value: that,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });
  const { hasAuth } = useAuth();

  Object.defineProperty(window, 'hasAuth', {
    value: hasAuth,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });

  Object.defineProperty(window, 'h', {
    value: h,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });
  // eslint-disable-next-line @typescript-eslint/no-unused-vars

  Object.defineProperty(window, 'UI', {
    value: UI,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  // Object.defineProperty(window, 'El', {
  //   value: El,
  //   configurable: true,
  //   enumerable: false, // 不会出现在 Object.keys 中
  //   writable: true
  // });
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  Object.defineProperty(window, '$t', {
    value: $t,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  Object.defineProperty(window, 'Enum', {
    value: Enum,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });

  Object.defineProperty(window, 'dayjs', {
    value: dayjs,
    configurable: true,
    enumerable: false, // 不会出现在 Object.keys 中
    writable: true
  });
  // eslint-disable-next-line no-eval
  return eval(`(${str || '{}'})`);
};
export default customRender;
