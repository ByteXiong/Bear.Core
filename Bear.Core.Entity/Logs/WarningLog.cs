using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Base;
using SqlSugar;

namespace Bear.Core.Entity.Logs
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
