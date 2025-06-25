using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.System.QuartzNet;
using Bear.Core.Models.Base;
using Bear.Core.SqlSugar;

namespace Bear.Core.Models;


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

