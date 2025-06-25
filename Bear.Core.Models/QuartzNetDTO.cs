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
/// QuartzNet作业服务
/// </summary>
#region 查询参数
public class QuartzNetParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(QuartzNetDTO), typeof(QuartzNet))]
public class QuartzNetDTO : RootKeyDTO<long>
{


}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(QuartzNetInfo), typeof(QuartzNet))]
public class QuartzNetInfo : QuartzNet
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateQuartzNetParam), typeof(QuartzNet))]
public class UpdateQuartzNetParam : QuartzNet
{
}
#endregion 


