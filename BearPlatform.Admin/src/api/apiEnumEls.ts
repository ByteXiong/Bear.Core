import { ColumnTypeEnum, MenuTypeEnum } from './apiEnums';

// 自定义组件
export const SearchComponent: Record<ColumnTypeEnum | string, string> = {
  [ColumnTypeEnum.文本]: 'ElInput',
  [ColumnTypeEnum.单图]: 'ElSwitch',
  [ColumnTypeEnum.多图]: 'ElInput',
  [ColumnTypeEnum.字典]: 'ElInput',
  [ColumnTypeEnum.富文本]: 'ElInput',
  [ColumnTypeEnum.整数]: 'ElInputNumber',
  [ColumnTypeEnum.枚举]: 'ElInput',
  [ColumnTypeEnum.小数]: 'ElInput',
  [ColumnTypeEnum.日期]: 'ElInput',
  [ColumnTypeEnum.时间]: 'ElInput',
  [ColumnTypeEnum.时间戳转当地日期]: 'ElInput',
  [ColumnTypeEnum.时间戳转当地时间]: 'ElInput',
  [ColumnTypeEnum.文件]: 'ElInput',
  [ColumnTypeEnum.布尔]: 'ElInput',
  [ColumnTypeEnum.颜色]: 'ElInput',
  [ColumnTypeEnum.TexTarea文本]: 'ElInput',
  [ColumnTypeEnum.自定义]: 'ElInput'
};
export const ColumnComponent: Record<ColumnTypeEnum, string> = {
  [ColumnTypeEnum.文本]: 'ElInput',
  [ColumnTypeEnum.单图]: 'ElInput',
  [ColumnTypeEnum.多图]: 'ElInput',
  [ColumnTypeEnum.字典]: 'ElInput',
  [ColumnTypeEnum.富文本]: 'ElInput',
  [ColumnTypeEnum.整数]: 'ElInput',
  [ColumnTypeEnum.枚举]: 'ElInput',
  [ColumnTypeEnum.小数]: 'ElInput',
  [ColumnTypeEnum.日期]: 'ElInput',
  [ColumnTypeEnum.时间]: 'ElInput',
  [ColumnTypeEnum.时间戳转当地日期]: 'ElInput',
  [ColumnTypeEnum.时间戳转当地时间]: 'ElInput',
  [ColumnTypeEnum.文件]: 'ElInput',
  [ColumnTypeEnum.布尔]: 'ElInput',
  [ColumnTypeEnum.颜色]: 'ElInput',
  [ColumnTypeEnum.TexTarea文本]: 'ElInput',
  [ColumnTypeEnum.自定义]: 'ElInput'
};

export const MenuTypeEl: Record<MenuTypeEnum, 'warning' | 'danger' | 'primary' | 'info' | 'success'> = {
  [MenuTypeEnum.Directory]: 'primary',
  [MenuTypeEnum.Menu]: 'primary',
  [MenuTypeEnum.Button]: 'primary',
  [MenuTypeEnum.Query]: 'primary'
};
