using Bear.Core.Common.Attributes;
using Bear.Core.Common.Model;
using SqlSugar;

namespace Bear.Core.Models.Queries.Permission;

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
