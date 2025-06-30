using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Logs;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.ViewModel;

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


