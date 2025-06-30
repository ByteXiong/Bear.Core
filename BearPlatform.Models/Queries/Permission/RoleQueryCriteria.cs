using BearPlatform.Common.Attributes;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.Permission;

/// <summary>
/// 角色查询参数
/// </summary>
public class RoleQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Name { get; set; }
}
