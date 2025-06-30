using BearPlatform.Common.Attributes;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.Permission;

/// <summary>
/// 部门查询参数
/// </summary>
public class DeptQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 部门名称
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Name { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public bool? Enabled { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal, IsGreaterThanNumberDefault = false)]
    public long ParentId { get; set; }
}
