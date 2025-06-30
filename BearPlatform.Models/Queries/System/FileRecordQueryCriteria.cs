using BearPlatform.Common.Attributes;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.System;

/// <summary>
/// 文件记录查询参数
/// </summary>
public class FileRecordQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 关键字
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like, FieldNameItems = ["Description", "OriginalName"])]
    public string KeyWords { get; set; }
}
