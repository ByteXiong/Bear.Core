using System;
using SqlSugar;

namespace BearPlatform.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class QueryConditionAttribute : Attribute
{
    /// <summary>
    /// 条件类型
    /// </summary>
    public ConditionalType ConditionType { get; set; }

    /// <summary>
    /// 指定字段名称 请求参数与实体参数名称不一致时使用
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// 指定字段名称,一个请求参数对应对应实体字段时使用
    /// </summary>
    public string[] FieldNameItems { get; set; } = [];

    /// <summary>
    /// 是否大于数字默认值才添加条件模型 例如int long默认0根据情况判断
    /// </summary>
    public bool IsGreaterThanNumberDefault { get; set; } = true;
}
