using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Logs
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
