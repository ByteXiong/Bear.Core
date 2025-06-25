using System;
using System.Collections.Generic;
using Bear.Core.Common.Attributes;
using SqlSugar;

namespace Bear.Core.Common.Model;

/// <summary>
/// 日期范围
/// </summary>
public class DateRange
{
    /// <summary>
    /// 开始[0]--结束[1]
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Range)]
    public List<DateTime> CreateTime { get; set; }
}
