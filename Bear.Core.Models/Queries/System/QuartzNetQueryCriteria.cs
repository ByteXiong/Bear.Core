using Bear.Core.Common.Attributes;
using Bear.Core.Common.Model;
using SqlSugar;

namespace Bear.Core.Models.Queries.System;

/// <summary>
/// 任务调度查询参数
/// </summary>
public class QuartzNetQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 任务名称
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like)]
    public string TaskName { get; set; }
}
