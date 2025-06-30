using BearPlatform.Common.Attributes;
using SqlSugar;

namespace BearPlatform.Models.Queries.System;

/// <summary>
/// 字典详情查询参数
/// </summary>
public class DictDetailQueryCriteria : IConditionalModel
{
    /// <summary>
    /// 字典ID
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public long DictId { get; set; }


    /// <summary>
    /// 字典ID
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public string DictName { get; set; }


    /// <summary>
    /// 标题
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Label { get; set; }
}
