using Bear.Core.Common.Attributes;
using Bear.Core.Common.Model;
using SqlSugar;

namespace Bear.Core.Models.Queries.System;

/// <summary>
/// 日志查询参数
/// </summary>
public class LogQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 关键字
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like, FieldName = "Description")]
    public string KeyWords { get; set; }
}
