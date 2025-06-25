using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Base;
using SqlSugar;

namespace Bear.Core.Entity.Logs
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
