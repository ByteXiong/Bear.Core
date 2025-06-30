using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Logs
{
    /// <summary>
    /// 信息日志
    /// </summary>
    [LogDataBase]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{"log_information"}_{{year}}{{month}}{{day}}")]
    public class InformationLog : SerilogBase
    {
    }
}
