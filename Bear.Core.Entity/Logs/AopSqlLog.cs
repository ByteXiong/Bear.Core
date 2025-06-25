using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Base;
using SqlSugar;

namespace Bear.Core.Entity.Logs
{
    /// <summary>
    /// SQL日志
    /// </summary>
    [LogDataBase]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{"log_sql"}_{{year}}{{month}}{{day}}")]
    public class AopSqlLog : SerilogBase
    {
    }
}
