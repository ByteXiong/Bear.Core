import ElInputConfig from './el-input-config';

/** 组件配置类型 */
export type ComponentsConfigType = {
  [key: string]: ComponentApi;
};

/** 所有组件的配置 */
export const componentsConfig: ComponentsConfigType = {
  ElInput: ElInputConfig
};

/** 组件选项接口 */
export type ComponentOption = {
  description: string;
  value: string | boolean | number;
};

/** 事件接口 */
export type EventItem = {
  event: string;
  description: string;
};

/** 配置项接口 */
export type Attributes = {
  prop: string;
  description: string;
  component: string;
  placeholder: string;
  options?: ComponentOption[];
};

export type Exposes = {
  slot: string;
  description: string;
  api: ComponentApi;
};

/** 组件配置接口 */
export type ComponentApi = {
  component: string;
  description: string;
  attributes: Attributes;
  events?: EventItem[];
  exposes: Exposes[];
};
