using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Base;
using SqlSugar;

namespace Bear.Core.Entity.Logs
{
    /// <summary>
    /// 失败日志
    /// </summary>
    [LogDataBase]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{"log_fatal"}_{{year}}{{month}}{{day}}")]
    public class FatalLog : SerilogBase
    {
    }
}
