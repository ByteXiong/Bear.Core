using BearPlatform.Common.Attributes;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.System;

/// <summary>
/// 全局设置查询参数
/// </summary>
public class SettingQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 关键字
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like, FieldNameItems = ["Name", "Value", "Description"])]
    public string KeyWords { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public bool? Enabled { get; set; }
}
