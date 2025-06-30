using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System.QuartzNet;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;


/// <summary>
/// QuartzNet作业日志服务
/// </summary>
#region 查询参数
public class QuartzNetLogParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(QuartzNetLogDTO), typeof(QuartzNetLog))]
public class QuartzNetLogDTO : RootKeyDTO<Guid>
{


}

/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateQuartzNetLogParam), typeof(QuartzNetLog))]
public class UpdateQuartzNetLogParam : QuartzNetLog
{
}
#endregion 

