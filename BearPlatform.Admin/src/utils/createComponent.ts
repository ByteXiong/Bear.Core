import { Component, createApp, DefineComponent, EmitsOptions } from "vue";

type Data = Record<string, unknown>;

/**
 * 推断组件的事件参数类型
 * @template T - 组件类型
 */
type EventParams<T> = T extends DefineComponent<any, any, any, any, any, any, any, infer Emits extends EmitsOptions>
  ? Emits extends Record<string, (...args: infer Args) => any>
    ? { [K in keyof Emits]: Parameters<Emits[K]> }
    : Emits extends string[]
      ? { [K in Emits[number]]: any[] }
      : { [key: string]: any[] }
  : { [key: string]: any[] };

/**
 * 创建一个 Vue 组件实例
 * @template T - 组件类型
 * @param {T} rootComponent - 要创建的根组件
 * @param {Data | null} [rootProps] - 传递给组件的 props
 * @returns {Promise<{
 *   instance: T;
 *   on: <K extends keyof EventParams<T>>(
 *     event: K,
 *     handler: (...args: EventParams<T>[K]) => void
 *   ) => void;
 *   unmount: () => void;
 * }>} - 返回包含组件实例、事件监听和卸载方法的对象
 */
async function createComponent<T extends Component>(
  rootComponent: T,
  rootProps?: Data | null
): Promise<{
  instance: T;
  on: <K extends keyof EventParams<T>>(
    event: K,
    handler: (...args: EventParams<T>[K]) => void
  ) => void;
  unmount: () => void;
}> {
  const mountNode = document.createElement('div');
  const app = createApp(rootComponent, rootProps);

  // 事件监听器存储
  const listeners = new Map<string, ((...args: any[]) => void)[]>();

  // 挂载实例
  const instance = app.mount(mountNode) as any;
  document.body.appendChild(mountNode);

  // 劫持 emit 方法
  const originalEmit = instance.$.emit;
  instance.$.emit = (event: string, ...args: any[]) => {
    originalEmit(event, ...args);
    listeners.get(event)?.forEach(fn => fn(...args));
  };

  return {
    instance: instance as T,
    on: (event, handler) => {
      const e = event as string;
      const h = handler as (...args: any[]) => void;
      const handlers = listeners.get(e) || [];
      listeners.set(e, [...handlers, h]);
    },
    unmount: () => {
      app.unmount();
      document.body.removeChild(mountNode);
      console.log("unmount");

    }
  };
}

export default createComponent;
