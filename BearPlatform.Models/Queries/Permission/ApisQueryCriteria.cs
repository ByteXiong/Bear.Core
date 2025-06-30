using BearPlatform.Common.Attributes;
using SqlSugar;

namespace BearPlatform.Models.Queries.Permission;

/// <summary>
/// 
/// </summary>
public class ApisQueryCriteria : IConditionalModel
{
    /// <summary>
    /// 组名称
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Group { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Description { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public string Method { get; set; }
}
