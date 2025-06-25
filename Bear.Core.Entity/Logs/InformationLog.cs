using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Base;
using SqlSugar;

namespace Bear.Core.Entity.Logs
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
