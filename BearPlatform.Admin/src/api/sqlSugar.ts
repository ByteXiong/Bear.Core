export enum ConditionalType {
  Equal, // 等于
  Like, // 模糊查询
  GreaterThan, // 大于
  GreaterThanOrEqual, // 大于等于
  LessThan, // 小于
  LessThanOrEqual, // 小于等于
  In, // In操作
  NotIn, //	Not in操作 参数和in一样
  LikeLeft, // 左模糊
  LikeRight, // 右模糊
  NoEqual, // 不等于
  IsNullOrEmpty, // 是null或者''
  IsNot, // 情况1   value不等于null
  NoLike, // 模糊查询取反
  EqualNull,
  InLike,
  Range // 区间
}
