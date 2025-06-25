using Bear.Core.Common.Attributes;
using Bear.Core.Common.Model;
using SqlSugar;

namespace Bear.Core.Models.Queries.System;

/// <summary>
/// 任务调度日志查询参数
/// </summary>
public class QuartzNetLogQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 任务ID
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal, FieldName = "TaskId")]
    public long Id { get; set; }

    /// <summary>
    /// 是否执行成功
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Equal)]
    public bool? IsSuccess { get; set; }
}
