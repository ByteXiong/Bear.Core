using BearPlatform.Common.Attributes;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.Permission;

/// <summary>
/// 岗位查询参数
/// </summary>
public class JobQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Name { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public bool? Enabled { get; set; }
}
