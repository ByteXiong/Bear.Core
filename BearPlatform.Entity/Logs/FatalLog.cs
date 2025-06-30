using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Logs
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
