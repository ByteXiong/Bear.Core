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


