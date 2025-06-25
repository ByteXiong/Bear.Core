using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Logs;
using Bear.Core.Models.Base;
using Bear.Core.SqlSugar;

namespace Bear.Core.ViewModel;

/// <summary>
/// 系统日志
/// </summary>
#region 查询参数
public class ExceptionLogParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(ExceptionLogDTO), typeof(ExceptionLog))]
public class ExceptionLogDTO : RootKeyDTO<Guid>
{


}

/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateExceptionLogParam), typeof(ExceptionLog))]
public class UpdateExceptionLogParam : ExceptionLog
{
}
#endregion


