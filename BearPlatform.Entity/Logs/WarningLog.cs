using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Logs
{
    /// <summary>
    /// 警告日志
    /// </summary>
    [LogDataBase]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{"log_warning"}_{{year}}{{month}}{{day}}")]
    public class WarningLog : SerilogBase
    {
    }
}
