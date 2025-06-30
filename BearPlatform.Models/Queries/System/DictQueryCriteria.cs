using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.System;

/// <summary>
/// 字典查询参数
/// </summary>
public class DictQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 关键字
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like, FieldNameItems = ["Name", "Description"])]
    public string KeyWords { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public DictType? DictType { get; set; }
}
