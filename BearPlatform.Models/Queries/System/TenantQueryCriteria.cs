using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.System;

/// <summary>
/// 租户查询参数
/// </summary>
public class TenantQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string Name { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public TenantType? TenantType { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public DbType? DbType { get; set; }
}
