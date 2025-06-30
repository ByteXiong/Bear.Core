using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Logs
{
    /// <summary>
    /// 错误日志
    /// </summary>
    [LogDataBase]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{"log_error"}_{{year}}{{month}}{{day}}")]
    public class ErrorLog : SerilogBase
    {
    }
}
