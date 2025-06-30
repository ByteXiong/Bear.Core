using BearPlatform.Common.Attributes;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Models.Queries.System;

/// <summary>
/// 密钥查询参数
/// </summary>
public class AppsecretQueryCriteria : DateRange, IConditionalModel
{
    /// <summary>
    /// 关键字
    /// </summary>
    [QueryCondition(ConditionType = ConditionalType.Like, FieldNameItems = ["AppId", "AppName", "Remark"])]
    public string KeyWords { get; set; }
}
